using WorkSphere.EmployeeService.Clients;
using WorkSphere.EmployeeService.DTOs;
using WorkSphere.EmployeeService.Models;
using WorkSphere.EmployeeService.Repositories;
using WorkSphere.EmployeeService.Exceptions;
using WorkSphere.EmployeeService.Messaging;

namespace WorkSphere.EmployeeService.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly DepartmentClient _departmentClient;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        private readonly RedisCacheService _redisCacheService;

        public EmployeeService(
            IEmployeeRepository repository,
            DepartmentClient departmentClient,
            RabbitMqPublisher rabbitMqPublisher,
            RedisCacheService redisCacheService)
        {
            _repository = repository;
            _departmentClient = departmentClient;
            _rabbitMqPublisher = rabbitMqPublisher;
            _redisCacheService = redisCacheService;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            const string cacheKey = "employees:all";
            
            var cachedEmployees =
                await _redisCacheService.GetAsync<List<Employee>>(cacheKey);

            if (cachedEmployees != null)
            {
                Console.WriteLine("Employees loaded from Redis cache.");
                return cachedEmployees;
            }

            var employees = await _repository.GetAllAsync();

            await _redisCacheService.SetAsync(
                cacheKey,
                employees
            );

            Console.WriteLine("Employees loaded from SQL Server and cached in Redis.");

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Employee> CreateAsync(CreateEmployeeDto dto)
        {
            // Check whether department exists
            var departmentExists =
                await _departmentClient.DepartmentExistsAsync(dto.Department);

            if (!departmentExists)
            {
                throw new BadRequestException(
                    $"Department '{dto.Department}' does not exist."
                );
            }

            var employee = new Employee
            {
                Name = dto.Name,
                Department = dto.Department,
                Age = dto.Age,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var createdEmployee =
                await _repository.CreateAsync(employee);

            // Publish employee-created event
            await _rabbitMqPublisher.PublishAsync(
                $"Employee created: {createdEmployee.Id} - {createdEmployee.Name}"
            );
            await _redisCacheService.RemoveAsync("employees:all");

            return createdEmployee;
        }

        public async Task<Employee?> UpdateAsync(
            int id,
            UpdateEmployeeDto dto)
        {
            var employee = await _repository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new NotFoundException(
                    $"Employee with ID {id} not found."
                );
            }

            // Check whether department exists
            var departmentExists =
                await _departmentClient.DepartmentExistsAsync(dto.Department);

            if (!departmentExists)
            {
                throw new BadRequestException(
                    $"Department '{dto.Department}' does not exist."
                );
            }

            employee.Name = dto.Name;
            employee.Department = dto.Department;
            employee.Age = dto.Age;
            employee.Email = dto.Email;
            employee.Phone = dto.Phone;

            var updatedEmployee =
                await _repository.UpdateAsync(employee);

            // Publish employee-updated event
            await _rabbitMqPublisher.PublishAsync(
                $"Employee updated: {updatedEmployee!.Id} - {updatedEmployee.Name}"
            );

            await _redisCacheService.RemoveAsync("employees:all");

            return updatedEmployee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
            {
                throw new NotFoundException(
                    $"Employee with ID {id} not found."
                );
            }

            // Publish employee-deleted event
            await _rabbitMqPublisher.PublishAsync(
                $"Employee deleted: {id}"
            );

            await _redisCacheService.RemoveAsync("employees:all");

            return true;
        }
    }
}
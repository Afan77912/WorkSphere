using WorkSphere.EmployeeService.DTOs;
using WorkSphere.EmployeeService.Models;

namespace WorkSphere.EmployeeService.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(CreateEmployeeDto dto);
        Task<Employee?> UpdateAsync(int id, UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
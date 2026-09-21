using Microsoft.EntityFrameworkCore;
using WorkSphere.EmployeeService.Data;
using WorkSphere.EmployeeService.Models;

namespace WorkSphere.EmployeeService.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Department = employee.Department;
            existingEmployee.Age = employee.Age;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;

            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
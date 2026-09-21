using WorkSphere.EmployeeService.Models;

namespace WorkSphere.EmployeeService.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee?> UpdateAsync(Employee employee);
        Task<bool> DeleteAsync(int id);
    }
}

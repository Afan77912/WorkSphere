using Microsoft.EntityFrameworkCore;
using WorkSphere.DepartmentService.Data;
using WorkSphere.DepartmentService.Models;

namespace WorkSphere.DepartmentService.Repositories
{
    public class DepartmentRepository
    {
        private readonly DepartmentDbContext _context;

        public DepartmentRepository(DepartmentDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }
               
        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == name);
        }
        public async Task<Department> CreateAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return department;
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}

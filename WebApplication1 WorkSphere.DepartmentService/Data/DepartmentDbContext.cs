using Microsoft.EntityFrameworkCore;
using WorkSphere.DepartmentService.Models;

namespace WorkSphere.DepartmentService.Data
{
    public class DepartmentDbContext : DbContext
    {
        public DepartmentDbContext(
            DbContextOptions<DepartmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
    }
}
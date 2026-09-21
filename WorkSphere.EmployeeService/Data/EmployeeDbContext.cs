using Microsoft.EntityFrameworkCore;
using WorkSphere.EmployeeService.Models;

namespace WorkSphere.EmployeeService.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}

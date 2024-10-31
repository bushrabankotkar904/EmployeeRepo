using EmpMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpMVC.Data
{
    public class EmployeeDbContext: DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options): base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}

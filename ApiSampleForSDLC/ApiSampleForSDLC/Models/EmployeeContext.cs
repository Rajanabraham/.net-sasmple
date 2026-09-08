using Microsoft.EntityFrameworkCore;

namespace ApiSampleForSDLC.Models
{
    /// <summary>
    /// EF Core DbContext for the Employee domain.
    /// </summary>
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="Employee"/> entities.
        /// </summary>
        public DbSet<Employee> Employees => Set<Employee>();
    }
}

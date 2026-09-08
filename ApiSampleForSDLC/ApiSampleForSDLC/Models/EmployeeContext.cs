using Microsoft.EntityFrameworkCore;

namespace ApiSampleForSDLC.Models
{
    /// <summary>
    /// Represents the EF Core context for the Employee domain.
    /// The application stores data in‑memory (via InMemory provider) but the
    /// context can be swapped for a relational provider without changing the
    /// controller logic.
    /// </summary>
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}

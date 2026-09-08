using Microsoft.EntityFrameworkCore;

namespace ApiSampleForSDLC.Models;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Email> Emails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure one‑to‑many relationship between Employee and Email
        modelBuilder.Entity<Email>()
            .HasOne(e => e.Employee)
            .WithMany(emp => emp.Emails)
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

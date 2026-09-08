using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSampleForSDLC.Models;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    // Existing fields (e.g., Department, Salary) omitted for brevity

    // Navigation property – an employee can have many email addresses
    public ICollection<Email> Emails { get; set; } = new List<Email>();
}

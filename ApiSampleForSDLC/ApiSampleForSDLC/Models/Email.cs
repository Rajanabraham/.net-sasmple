using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSampleForSDLC.Models;

public class Email
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string EmailAddress { get; set; } = null!;

    public bool IsPrimary { get; set; }

    // Foreign key to Employee
    [ForeignKey(nameof(Employee))]
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}

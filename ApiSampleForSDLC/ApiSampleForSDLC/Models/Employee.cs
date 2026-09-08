using System.ComponentModel.DataAnnotations;

namespace ApiSampleForSDLC.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(18, 70)]
        public int Age { get; set; }

        // New Email field – optional but validated when provided
        [EmailAddress]
        public string? Email { get; set; }
    }
}
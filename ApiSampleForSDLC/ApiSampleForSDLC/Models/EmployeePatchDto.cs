using System.ComponentModel.DataAnnotations;

namespace ApiSampleForSDLC.Models
{
    /// <summary>
    /// DTO for PATCH requests. All properties are nullable because a client may send any subset.
    /// </summary>
    public class EmployeePatchDto
    {
        public string? Name { get; set; }
        public int? Age { get; set; }

        // New Email field for patching
        [EmailAddress]
        public string? Email { get; set; }
    }
}
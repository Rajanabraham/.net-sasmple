using System.ComponentModel.DataAnnotations;

namespace ApiSampleForSDLC.Models;

public class EmailDto
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string EmailAddress { get; set; } = null!;

    public bool IsPrimary { get; set; }
}

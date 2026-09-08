using System.ComponentModel.DataAnnotations;

namespace ApiSampleForSDLC.Models;

public class CreateUserDto
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

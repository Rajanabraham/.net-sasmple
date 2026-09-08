using System.ComponentModel.DataAnnotations;

namespace ApiSampleForSDLC.Models;

public class UpdateUserDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

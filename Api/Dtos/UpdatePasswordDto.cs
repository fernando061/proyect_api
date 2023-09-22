using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;
public class UpdatePasswordDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
}


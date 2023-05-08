using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.Models;

public class LoginViewModel
{
    // Validation annotation
    [Display(Name = "EmailAddress")]
    [Required(ErrorMessage = "Email address is required")]
    public string EmailAddress { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

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

    public string ReturnUrl { get; set; }
    public IList<AuthenticationScheme> ExternalLogins { get; set; }
}
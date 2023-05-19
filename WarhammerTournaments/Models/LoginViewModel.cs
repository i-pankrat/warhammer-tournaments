using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace WarhammerTournaments.Models;

public class LoginViewModel
{
    // Validation annotation
    [Display(Name = "Почта")]
    [Required(ErrorMessage = "Необходимо ввести почту")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Необходимо ввести пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }
    public IEnumerable<AuthenticationScheme> ExternalLogins { get; set; }
}
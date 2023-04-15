using System.ComponentModel.DataAnnotations;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace WarhammerTournaments.ViewModels;

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
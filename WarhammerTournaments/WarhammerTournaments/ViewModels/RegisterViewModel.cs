using System.ComponentModel.DataAnnotations;

namespace WarhammerTournaments.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Email address")]
    [Required(ErrorMessage = "Email address is required")]
    public string EmailAddress { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password address is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("Password", ErrorMessage = "Password do not match")]
    public string ConfirmPassword { get; set; }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WarhammerTournaments.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Email address")]
    [Required(ErrorMessage = "Необходимо ввести почту")]
    [Remote("IsEmailAddressExist", "Account", HttpMethod = "post", ErrorMessage = "Почта уже зарегистрирована")]
    public string EmailAddress { get; set; }

    [Display(Name = "Username")]
    [Required(ErrorMessage = "Необходимо ввести имя")]
    [Remote("IsUserNameExist", "Account", HttpMethod = "post", ErrorMessage = "Имя пользователя занято")]
    public string Username { get; set; }

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Необходимо ввести пароль")]
    [Remote("IsPasswordValid", "Account", HttpMethod = "post")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Необходимо повторить пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }
    public IEnumerable<AuthenticationScheme> ExternalLogins { get; set; }
}
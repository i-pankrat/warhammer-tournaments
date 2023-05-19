using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User.ManagementService.Models;
using User.ManagementService.Services;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.Models;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _roleManager = roleManager;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<JsonResult> IsEmailAddressExist(string emailAddress)
    {
        var user = await _userManager.FindByEmailAsync(emailAddress);
        return new JsonResult(user == null);
    }

    [HttpPost]
    public async Task<JsonResult> IsUserNameExist(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return new JsonResult(user == null);
    }

    [HttpPost]
    public JsonResult IsPasswordValid(string password)
    {
        if (password.Length < 8)
        {
            return Json("Пароль должен содержать минимум 8 символов");
        }

        return Json(true);
    }

    [HttpPost]
    public async Task<JsonResult> PasswordValidation(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return new JsonResult(user != null);
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        var response = new LoginViewModel
        {
            ReturnUrl = Url.Action("Index", "Tournaments"),
            ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync()
        };
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid) return View(loginViewModel);

        var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

        if (user != null)
        {
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Вы вошли!";
                    return RedirectToAction("Index", "Tournaments");
                }

                ViewData["Fail"] = "Скорее всего вы не подтвердили свою почту!";
                return View(loginViewModel);
            }

            /*
             * Replace with additional logic in class
             * That's bad pattern to use TempData (LoginViewModel.PasswordIsCorrect ... ?)
             */

            return View(loginViewModel);
        }

        ViewData["Fail"] = "Неверная почта или пароль. Попробуйте снова!";

        return View(loginViewModel);
        // return RedirectToAction("Register");
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        var response = new RegisterViewModel
        {
            ReturnUrl = Url.Action("Index", "Tournaments"),
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
        };

        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid)
            return View(registerViewModel);


        var userEmail = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
        var userName = await _userManager.FindByNameAsync(registerViewModel.Username);

        // Check if user exists
        if (userEmail != null || userName != null)
        {
            // Not the best way of dealing with the situation
            TempData["Error"] = "This email or username is already in use";

            return View(registerViewModel);
        }


        var newUser = new ApplicationUser
        {
            Email = registerViewModel.EmailAddress,
            UserName = registerViewModel.Username
        };

        var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

        if (newUserResponse.Succeeded)
        {
            // Check if the role was added ?
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            // Email verification: add token and send email for confirmation
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = newUser.Email },
                Request.Scheme);
            var message = new Message(new List<string> { newUser.Email },
                "Confirmation email link for warhammer tournaments",
                confirmationLink);

            _emailService.SentEmail(message);
        }
        else
        {
            TempData["Error"] = "Failed to create user";
            return View(registerViewModel);
        }

        TempData["Success"] = "Вам удалось зарегистрироваться, подтвердите свою почту!";
        return RedirectToAction("Index", "Tournaments");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Tournaments");
    }

    [HttpGet("ConfirmEmail")] // Any [Authorize]?
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);

            // Maybe add something to say that email has been confirmed
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
        }

        TempData["Error"] = "Failed to confirm email. Try again, please!";
        return RedirectToAction("Register");
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
            new { ReturnUrl = returnUrl });

        var properties =
            _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return new ChallengeResult(provider, properties);
    }

    [AllowAnonymous]    
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        var registerViewModel = new RegisterViewModel()
        {
            ReturnUrl = returnUrl,
            ExternalLogins =
                (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };

        if (remoteError != null)
        {
            ModelState
                .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

            return View("Register", registerViewModel);
        }

        // Get the login information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ModelState
                .AddModelError(string.Empty, "Error loading external login information.");

            return View("Register", registerViewModel);
        }

        // If the user already has a login (i.e if there is a record in AspNetUserLogins
        // table) then sign-in the user with this external login provider
        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
            info.ProviderKey, false, false);

        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        // If there is no record in AspNetUserLogins table, the user may not have
        // a local account
        else
        {
            // Get the email claim value
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {
                // Create a new user without password if we do not have a user already
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    var userName = email.Split("@").First();
                    var counter = 1;

                    while (await _userManager.FindByNameAsync(userName) != null)
                    {
                        userName = String.Concat(userName, counter);
                        counter++;
                    }
                    
                    user = new ApplicationUser
                    {
                        UserName = userName,
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };

                    await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, UserRoles.User);
                }

                // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);

                return LocalRedirect(returnUrl);
            }

            ViewData["Fail"] = "У вас не привяза почта к аккаунту ВКонтакте, к сожалнию вы не можете войти!";
            return RedirectToAction("Register");
        }
    }
}
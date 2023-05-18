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
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        var response = new LoginViewModel(); // In case of reloading
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
                    return RedirectToAction("Index", "Tournaments");
                }
            }

            /*
             * Replace with additional logic in class
             * That's bad pattern to use TempData (LoginViewModel.PasswordIsCorrect ... ?)
             */

            TempData["Error"] = "Wrong credentials. Please, try again";
            return View(loginViewModel);
        }

        TempData["Error"] = "Wrong credentials. Please, try again";

        return View(loginViewModel);
        // return RedirectToAction("Register");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        var response = new RegisterViewModel();
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
}
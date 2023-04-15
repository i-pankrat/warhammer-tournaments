using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.Data;
using WarhammerTournaments.Models;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    // GET
    public IActionResult Login()
    {
        var response = new LoginViewModel(); // In case of reloading
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
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
}
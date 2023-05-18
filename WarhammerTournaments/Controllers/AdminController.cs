using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.Models;

namespace WarhammerTournaments.Controllers;

[Authorize(Roles = $"{UserRoles.Admin}")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Users()
    {
        var usersViewModel = new ApplicationUsersViewModel { Users = await _userManager.Users.ToListAsync() };
        return View(usersViewModel);
    }

    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user != null)
        {
            await _userManager.DeleteAsync(user);
        }

        return RedirectToAction("Users");
    }

    public async Task<IActionResult> MakeAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }

        return RedirectToAction("Users");
    }

    public async Task<IActionResult> MakeOrganizer(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user != null)
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Organizer);
        }

        return RedirectToAction("Users");
    }
}
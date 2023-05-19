using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Attr;
using User.ManagementService.Models;
using User.ManagementService.Services;
using WarhammerTournaments.DAL;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.Models;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public ProfileController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<JsonResult> IsUserNameValid(string userName)
    {
        var allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.";

        if (!userName.All(x => allowed.Contains(x)))
        {
            return new JsonResult("Можно использовать буквы, цифры и следующие символы: .-_");
        }

        var userId = User.GetUserId();
        var currentUser = await _userManager.FindByIdAsync(userId);
        var user = await _userManager.FindByNameAsync(userName);

        if (user != null && currentUser.UserName != userName)
        {
            return new JsonResult("Данное имя пользователя занято");
        }

        return new JsonResult(user == null);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        var userId = User.GetUserId();
        var user = await _userManager.FindByIdAsync(userId);

        var profileVm = new ProfileViewModel
        {
            UserId = userId,
            UserName = user.UserName
        };

        return View(profileVm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(ProfileViewModel profileViewModel)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid) return View(profileViewModel);

        var user = await _userManager.FindByIdAsync(profileViewModel.UserId);
        user.UserName = profileViewModel.UserName;
        await _userManager.UpdateAsync(user);

        var tournaments = await _unitOfWork.TournamentRepository.Get(x => x.OwnerId == user.Id);
        var applications = await _unitOfWork.ApplicationRepository.Get(x => x.UserId == user.Id);

        foreach (var tournament in tournaments)
        {
            tournament.OwnerUserName = user.UserName;
            _unitOfWork.TournamentRepository.Update(tournament);
        }

        foreach (var application in applications)
        {
            application.UserName = user.UserName;
            _unitOfWork.ApplicationRepository.Update(application);
        }

        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction("Index", "Tournaments");
    }
}
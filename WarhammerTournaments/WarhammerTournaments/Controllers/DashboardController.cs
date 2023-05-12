using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.DAL;
using WarhammerTournaments.DAL.Data;

namespace WarhammerTournaments.Controllers;

[Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
public class DashboardController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var userTournaments = await _unitOfWork.UserRepository.GetAllUserTournaments();
        /*var dashboardViewModel = new DashboardViewModel
        {
            Tournaments = userTournaments
        };*/
        return View(userTournaments);
    }
}
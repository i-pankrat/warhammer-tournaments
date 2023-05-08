using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.DAL;

namespace WarhammerTournaments.Controllers;

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
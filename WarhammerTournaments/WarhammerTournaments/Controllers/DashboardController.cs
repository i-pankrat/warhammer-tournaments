using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.Data;
using WarhammerTournaments.Interfaces;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<IActionResult> Index()
    {
        var userTournaments = await _dashboardRepository.GetAllUserTournaments();
        var dashboardViewModel = new DashboardViewModel
        {
            Tournaments = userTournaments
        };
        return View(dashboardViewModel);
    }
}
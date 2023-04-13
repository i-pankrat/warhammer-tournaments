using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.Interfaces;

namespace WarhammerTournaments.Controllers;

public class TournamentsController : Controller
{
    private readonly ITournamentRepository _tournamentRepository;

    public TournamentsController(ITournamentRepository tournamentRepository)
    {
        _tournamentRepository = tournamentRepository;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var tournaments = await _tournamentRepository.GetAll();
        return View(tournaments);
    }
}
using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.Interfaces;

namespace WarhammerTournaments.Controllers;

public class DetailsController : Controller
{
    private readonly ITournamentRepository _tournamentRepository;

    public DetailsController(ITournamentRepository tournamentRepository)
    {
        _tournamentRepository = tournamentRepository;
    }

    // GET
    public async Task<IActionResult> Index(int id)
    {
        return View(await _tournamentRepository.GetByIdAsync(id));
    }
}
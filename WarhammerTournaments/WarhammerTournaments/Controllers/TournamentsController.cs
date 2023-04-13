using Microsoft.AspNetCore.Mvc;

namespace WarhammerTournaments.Controllers;

public class TournamentsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
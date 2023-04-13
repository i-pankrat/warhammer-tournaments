using Microsoft.AspNetCore.Mvc;

namespace WarhammerTournaments.Controllers;

public class DetailsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.Interfaces;
using WarhammerTournaments.Models;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

public class TournamentsController : Controller
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IImageUploadService _imageUploadService;

    public TournamentsController(ITournamentRepository tournamentRepository, IImageUploadService imageUploadService)
    {
        _tournamentRepository = tournamentRepository;
        _imageUploadService = imageUploadService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var tournaments = await _tournamentRepository.GetAll();
        return View(tournaments);
    }

    public async Task<IActionResult> Create(TournamentViewModel tournamentViewModel)
    {
        if (ModelState.IsValid)
        {
            var fileName = await _imageUploadService.Upload(tournamentViewModel.Image);
            var tournament = new Tournament
            {
                Title = tournamentViewModel.Title,
                Description = tournamentViewModel.Description,
                ParticipantNumber = tournamentViewModel.ParticipantNumber,
                Date = tournamentViewModel.Date,
                ImageName = fileName,
                EntranceFee = tournamentViewModel.EntranceFee
            };

            _tournamentRepository.Add(tournament);
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Photo upload failed");
        return View(tournamentViewModel);
    }
}
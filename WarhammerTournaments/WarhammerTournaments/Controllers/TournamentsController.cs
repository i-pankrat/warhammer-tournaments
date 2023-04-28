using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.Interfaces;
using WarhammerTournaments.Models;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

public class TournamentsController : Controller
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IImageUploadService _imageUploadService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TournamentsController(ITournamentRepository tournamentRepository, IImageUploadService imageUploadService,
        IHttpContextAccessor httpContextAccessor)
    {
        _tournamentRepository = tournamentRepository;
        _imageUploadService = imageUploadService;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var tournaments = await _tournamentRepository.GetAll();
        return View(tournaments);
    }

    public async Task<IActionResult> Details(int id)
    {
        return View(await _tournamentRepository.GetByIdAsync(id));
    }

    public IActionResult Create()
    {
        var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var tournamentViewModel = new TournamentViewModel
        {
            OwnerId = curUserId
        };
        return View(tournamentViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TournamentViewModel tournamentViewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _imageUploadService.UploadAsync(tournamentViewModel.Image);
            var tournament = new Tournament
            {
                OwnerId = tournamentViewModel.OwnerId,
                Title = tournamentViewModel.Title,
                Description = tournamentViewModel.Description,
                AvailableParticipant = tournamentViewModel.AvailableParticipant,
                RegisteredParticipant = 0,
                Users = new List<User>(),
                Address = tournamentViewModel.Address,
                Date = tournamentViewModel.Date,
                ImageUrl = result.url,
                ImageId = result.fileId,
                EntranceFee = tournamentViewModel.EntranceFee,
            };

            _tournamentRepository.Add(tournament);
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Failed to add new tournament");
        return View(tournamentViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Join(int id)
    {
        var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var applicationViewModel = new JoinViewModel()
        {
            UserId = curUserId,
            TournamentId = id
        };

        return View(applicationViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Join(JoinViewModel joinViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _tournamentRepository.GetUserByIdAsync(joinViewModel.UserId);
            var application = new Application
            {
                TournamentId = joinViewModel.TournamentId,
                UserId = user.Id,
                UserName = user.UserName,
                Elo = 0,
                Hin = 0,
                Fraction = joinViewModel.Fraction,
                Roster = joinViewModel.Roster,
            };

            _tournamentRepository.AddApplication(application);
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Failed to add application");
        return View(joinViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id);

        if (tournament == null)
            return View("Error");

        var tournamentVM = new TournamentViewModel
        {
            Id = tournament.Id,
            Title = tournament.Title,
            Description = tournament.Description,
            OwnerId = tournament.OwnerId,
            Address = tournament.Address,
            AvailableParticipant = tournament.AvailableParticipant,
            Date = tournament.Date,
            Image = null,
            EntranceFee = tournament.EntranceFee
        };

        return View(tournamentVM);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int id, TournamentViewModel tournamentViewModel)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit tournament");
            return View("Edit", tournamentViewModel);
        }

        var oldTournament = await _tournamentRepository.GetByIdAsyncNoTracking(tournamentViewModel.Id);
        if (oldTournament != null)
        {
            var deleteResult = await _imageUploadService.DeleteAsync(oldTournament.ImageId);
            var uploadResult = await _imageUploadService.UploadAsync(tournamentViewModel.Image);
            
            var tournament = new Tournament
            {
                Id = tournamentViewModel.Id,
                Title = tournamentViewModel.Title,
                Description = tournamentViewModel.Description,
                OwnerId = tournamentViewModel.OwnerId,
                Address = tournamentViewModel.Address,
                AvailableParticipant = tournamentViewModel.AvailableParticipant,
                Date = tournamentViewModel.Date,
                ImageUrl = uploadResult.url,
                ImageId = uploadResult.fileId,
                EntranceFee = tournamentViewModel.EntranceFee
            };

            _tournamentRepository.Update(tournament);
            return RedirectToAction("Index", "Dashboard");
        }

        return View(tournamentViewModel);
    }

    public async Task<IActionResult> Applications(int id)
    {
        var applications = await _tournamentRepository.GetApplicationsByTournamentIdAsync(id);
        var applicationsVM = new ApplicationViewModel
        {
            Applications = applications,
            TournamentId = id
        };
        return View(applicationsVM);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(id);

        if (tournament != null)
        {
            var deleteResult = await _imageUploadService.DeleteAsync(tournament.ImageId);
            _tournamentRepository.Delete(tournament);
            return RedirectToAction("Index", "Dashboard");
        }

        return View("Error");
    }

    public async Task<IActionResult> DeleteAllApplications(int id)
    {
        await _tournamentRepository.DeleteAllApplicationsByTournamentIdAsync(id);
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> AcceptApplication(int applicationId)
    {
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> RejectApplication(int applicationId)
    {
        return RedirectToAction("Index", "Dashboard");
    }
}
using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.DAL;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.Interfaces;
using WarhammerTournaments.Models;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

public class TournamentsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploadService _imageUploadService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TournamentsController(IUnitOfWork unitOfWork, IImageUploadService imageUploadService,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _imageUploadService = imageUploadService;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var tournaments = await _unitOfWork.TournamentRepository.GetAll();
        return View(tournaments);
    }

    public async Task<IActionResult> Details(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.Get(id);
        var applications =
            await _unitOfWork.ApplicationRepository.Get(x => x.TournamentId == tournament.Id & x.IsAccepted);
        tournament.Participants = applications == null
            ? new List<Application>()
            : applications.Where(a => a.IsAccepted).ToList();
        return View(tournament);
    }

    public async Task<IActionResult> Create()
    {
        var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _unitOfWork.UserRepository.Get(curUserId);
        var tournamentViewModel = new TournamentViewModel
        {
            OwnerUserName = user.UserName,
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
            var user = await _unitOfWork.UserRepository.Get(tournamentViewModel.OwnerId);
            var tournament = new Tournament
            {
                OwnerId = tournamentViewModel.OwnerId,
                OwnerUserName = user.UserName,
                Title = tournamentViewModel.Title,
                Description = tournamentViewModel.Description,
                AvailableParticipant = tournamentViewModel.AvailableParticipant,
                RegisteredParticipant = 0,
                Participants = new List<Application>(),
                Address = tournamentViewModel.Address,
                Date = tournamentViewModel.Date,
                ImageUrl = result.url,
                ImageId = result.fileId,
                EntranceFee = tournamentViewModel.EntranceFee,
            };

            await _unitOfWork.TournamentRepository.AddAsync(tournament);
            await _unitOfWork.SaveChangesAsync();
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
            var user = await _unitOfWork.UserRepository.Get(joinViewModel.UserId);
            var application = new Application
            {
                TournamentId = joinViewModel.TournamentId,
                UserId = user.Id,
                UserName = user.UserName,
                IsAccepted = false,
                Elo = 0,
                Hin = 0,
                Fraction = joinViewModel.Fraction,
                Roster = joinViewModel.Roster,
            };

            await _unitOfWork.ApplicationRepository.AddAsync(application);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ModelState.AddModelError("", "Failed to add application");
        return View(joinViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.Get(id);

        if (tournament == null)
            return View("Error");

        var tournamentVM = new TournamentViewModel
        {
            Id = tournament.Id,
            Title = tournament.Title,
            Description = tournament.Description,
            OwnerId = tournament.OwnerId,
            OwnerUserName = tournament.OwnerId,
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

        var oldTournament = await _unitOfWork.TournamentRepository.GetNoTracking(tournamentViewModel.Id);
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
                OwnerUserName = tournamentViewModel.OwnerUserName,
                Address = tournamentViewModel.Address,
                AvailableParticipant = tournamentViewModel.AvailableParticipant,
                Date = tournamentViewModel.Date,
                ImageUrl = uploadResult.url,
                ImageId = uploadResult.fileId,
                EntranceFee = tournamentViewModel.EntranceFee
            };

            _unitOfWork.TournamentRepository.Update(tournament);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard");
        }

        return View(tournamentViewModel);
    }

    public async Task<IActionResult> Applications(int id)
    {
        var applications = await _unitOfWork.ApplicationRepository
            .Get(x => x.TournamentId == id & !x.IsAccepted);
        var applicationsVM = new ApplicationViewModel
        {
            Applications = applications,
            TournamentId = id
        };
        return View(applicationsVM);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.Get(id);

        if (tournament != null)
        {
            var deleteResult = await _imageUploadService.DeleteAsync(tournament.ImageId);
            _unitOfWork.TournamentRepository.Remove(tournament);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Applications");
        }

        return View("Error");
    }

    public async Task<IActionResult> DeleteAllApplications(int id)
    {
        var applications = await _unitOfWork.ApplicationRepository.Get(x => x.TournamentId == id & !x.IsAccepted);
        _unitOfWork.ApplicationRepository.RemoveRange(applications);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction("Index", "Dashboard");
    }

    public async Task<IActionResult> AcceptApplication(int id)
    {
        var application = await _unitOfWork.ApplicationRepository.Get(id);
        var tournament = await _unitOfWork.TournamentRepository.Get(application.TournamentId);

        if (application != null)
        {
            if (tournament.RegisteredParticipant == tournament.AvailableParticipant)
            {
                // can not add new patricipant;
                return View("Error");
            }

            application.IsAccepted = true;
            tournament.RegisteredParticipant += 1;

            _unitOfWork.TournamentRepository.Update(tournament);
            _unitOfWork.ApplicationRepository.Update(application);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Applications", "Tournaments", new { @id = application.TournamentId });
        }

        return View("Error");
    }

    public async Task<IActionResult> RejectApplication(int id)
    {
        var application = await _unitOfWork.ApplicationRepository.Get(id);
        _unitOfWork.ApplicationRepository.Remove(application);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction("Applications", "Tournaments", new { @id = application.TournamentId });
    }
}
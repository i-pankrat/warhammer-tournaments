using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WarhammerTournaments.DAL;
using WarhammerTournaments.DAL.Data;
using WarhammerTournaments.DAL.Entity;
using WarhammerTournaments.Interfaces;
using WarhammerTournaments.Models;
using WarhammerTournaments.Services;
using WarhammerTournaments.ViewModels;

namespace WarhammerTournaments.Controllers;

public class TournamentsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageUploadService _imageUploadService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public TournamentsController(IUnitOfWork unitOfWork, IImageUploadService imageUploadService,
        IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _imageUploadService = imageUploadService;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tournaments = await _unitOfWork.TournamentRepository.GetAll();
        return View(tournaments.OrderBy(x => x.Date));
    }

    [AllowAnonymous]
    [HttpGet]
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

    [HttpGet]
    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> Create()
    {
        var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userManager.FindByIdAsync(curUserId);
        var tournamentViewModel = new TournamentViewModel
        {
            OwnerUserName = user.UserName,
            OwnerId = curUserId,
            Date = DateTime.Now
        };
        return View(tournamentViewModel);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> Create(TournamentViewModel tournamentViewModel)
    {
        if (ModelState.IsValid)
        {
            Result result;

            if (tournamentViewModel.Image != null)
            {
                result = await _imageUploadService.UploadAsync(tournamentViewModel.Image);
            }
            else
            {
                result = new Result
                {
                    url = ImageUploadService.DefaultImageUrl,
                    fileId = ""
                };
            }

            var user = await _userManager.FindByIdAsync(tournamentViewModel.OwnerId);
            var tournament = new Tournament
            {
                OwnerId = tournamentViewModel.OwnerId,
                OwnerUserName = user.UserName,
                Title = tournamentViewModel.Title,
                Description = tournamentViewModel.Description,
                Rules = tournamentViewModel.Rules,
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
    [Authorize(Roles = $"{UserRoles.User}, {UserRoles.Organizer}, {UserRoles.Admin}")]
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
    [Authorize(Roles = $"{UserRoles.User}, {UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> Join(JoinViewModel joinViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(joinViewModel.UserId);

            if (user == null)
            {
                return View(joinViewModel);
            }

            var userApplications = await _unitOfWork.ApplicationRepository.Get(x =>
                x.UserId == user.Id && x.TournamentId == joinViewModel.TournamentId);

            if (!userApplications.Any())
            {
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
            else
            {
                // TODO: Show error that user has already sent application
                return RedirectToAction("Index");
            }
        }

        ModelState.AddModelError("", "Failed to add application");
        return View(joinViewModel);
    }

    [HttpGet]
    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
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
    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
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
            var deleteResult = await _imageUploadService.DeleteAsync(oldTournament.ImageId, oldTournament.ImageUrl);
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

    [HttpGet]
    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
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

    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> ProcessApplication(int id, ApplicationViewModel viewModel, string action)
    {
        return action switch
        {
            "Принять" => await AcceptApplication(id, viewModel.AcceptedPlayerHin, viewModel.AcceptedPlayerElo),
            "Отклонить" => await Delete(id),
            _ => RedirectToAction("Applications")
        };
    }

    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tournament = await _unitOfWork.TournamentRepository.Get(id);

        if (tournament != null)
        {
            var deleteResult = await _imageUploadService.DeleteAsync(tournament.ImageId, tournament.ImageUrl);
            _unitOfWork.TournamentRepository.Remove(tournament);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Applications");
        }

        return View("Error");
    }

    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> DeleteAllApplications(int id)
    {
        var applications = await _unitOfWork.ApplicationRepository.Get(x => x.TournamentId == id & !x.IsAccepted);
        _unitOfWork.ApplicationRepository.RemoveRange(applications);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction("Index", "Dashboard");
    }

    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> AcceptApplication(int id, int hin, int elo)
    {
        var application = await _unitOfWork.ApplicationRepository.Get(id);
        if (application != null)
        {
            var tournament = await _unitOfWork.TournamentRepository.Get(application.TournamentId);

            if (tournament != null)
            {
                if (tournament.RegisteredParticipant == tournament.AvailableParticipant)
                {
                    // can not add new patricipant;
                    return View("Error");
                }

                application.IsAccepted = true;
                application.Hin = hin;
                application.Elo = elo;
                tournament.RegisteredParticipant += 1;

                _unitOfWork.TournamentRepository.Update(tournament);
                _unitOfWork.ApplicationRepository.Update(application);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Applications", "Tournaments", new { @id = application.TournamentId });
            }
        }

        return View("Error");
    }

    [Authorize(Roles = $"{UserRoles.Organizer}, {UserRoles.Admin}")]
    public async Task<IActionResult> RejectApplication(int id)
    {
        var application = await _unitOfWork.ApplicationRepository.Get(id);
        _unitOfWork.ApplicationRepository.Remove(application);
        await _unitOfWork.SaveChangesAsync();
        return RedirectToAction("Applications", "Tournaments", new { @id = application.TournamentId });
    }
}
using Microsoft.AspNetCore.Mvc;
using sportify.BLL.Services;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.PL.ViewModels;

public class TournamentController : Controller
{
    private readonly ITournamentService _tournamentService;

    public TournamentController(ITournamentService service)
    {
        _tournamentService = service;
    }

    public async Task<IActionResult> Index()
    {
        var tournaments = await _tournamentService.GetAllTournamentsAsync();

        var viewModel = tournaments.Select(t => new TournamentViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            LogoUrl = t.LogoUrl,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            Status = t.Status
        });

        return View(viewModel);
    }

    // GET: /Tournament/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Tournament/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tournament tournament)
    {
        if (ModelState.IsValid)
        {
            await _tournamentService.AddAsync(tournament);
            return RedirectToAction(nameof(Index));
        }
        return View(tournament);
    }

    // GET: /Tournament/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        if (tournament == null) return NotFound();

        var viewModel = new TournamentViewModel
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Description = tournament.Description,
            LogoUrl = tournament.LogoUrl,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Status = tournament.Status
        };

        return View(viewModel);
    }

    // POST: /Tournament/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TournamentViewModel model)
    {
        if (id != model.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            var tournament = new Tournament
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                LogoUrl = model.LogoUrl,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = model.Status
            };
            await _tournamentService.UpdateAsync(tournament);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: /Tournament/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        if (tournament == null) return NotFound();

        var viewModel = new TournamentViewModel
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Description = tournament.Description,
            LogoUrl = tournament.LogoUrl,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Status = tournament.Status
        };
        return View(viewModel);
    }

    // POST: /Tournament/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _tournamentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

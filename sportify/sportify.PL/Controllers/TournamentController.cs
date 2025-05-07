using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;

public class TournamentController : Controller
{
    private readonly ITournamentService _tournamentService;

    public TournamentController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    public async Task<IActionResult> Index()
    {
        var tournaments = await _tournamentService.GetAllAsync();

        return View(tournaments);
    }

    public async Task<IActionResult> Details(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        return tournament == null ? NotFound() : View(tournament);
    }

    // GET: /Tournament/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Tournament/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TournamentDTO model)
    {
        if (ModelState.IsValid)
        {
            await _tournamentService.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: /Tournament/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);
        return tournament == null ? NotFound() : View(tournament);
    }

    // POST: /Tournament/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TournamentDTO model)
    {
        if (id != model.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            await _tournamentService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // GET: /Tournament/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var tournament = await _tournamentService.GetByIdAsync(id);

        return tournament == null ? NotFound() : View(tournament);
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

using GoEASy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class TourController : Controller
{
    private readonly TourService _tourService;

    public TourController(TourService tourService)
    {
        _tourService = tourService;
    }

    // GET: /Tour
    public async Task<IActionResult> Index()
    {
        var tours = await _tourService.GetAllToursAsync();
        return View("~/Views/client/tour-list.cshtml", tours);
    }

    // GET: /Tour/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null) return NotFound();
        return View(tour);
    }

    // GET: /Tour/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Tour/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tour tour)
    {
        if (ModelState.IsValid)
        {
            await _tourService.AddTourAsync(tour);
            return RedirectToAction(nameof(Index));
        }
        return View(tour);
    }

    // GET: /Tour/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null) return NotFound();
        return View(tour);
    }

    // POST: /Tour/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tour tour)
    {
        if (id != tour.TourId) return NotFound();
        if (ModelState.IsValid)
        {
            await _tourService.UpdateTourAsync(tour);
            return RedirectToAction(nameof(Index));
        }
        return View(tour);
    }

    // GET: /Tour/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var tour = await _tourService.GetTourByIdAsync(id);
        if (tour == null) return NotFound();
        return View(tour);
    }

    // POST: /Tour/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _tourService.DeleteTourAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
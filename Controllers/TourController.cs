
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TourController : Controller
{
    private readonly GoEasyContext _context;
    private readonly TourLogService _tourLogService;

    public TourController(GoEasyContext context, TourLogService tourLogService)
    {
        _context = context;
        _tourLogService = tourLogService;
    }

    public async Task<IActionResult> Detail(int id)
    {
        var tour = await _context.Tours
            .Include(t => t.TourImages)
            .Include(t => t.Destination)
            .FirstOrDefaultAsync(t => t.TourID == id);

        int? userId = HttpContext.Session.GetInt32("UserID");

        Console.WriteLine("CALL LOG: TourId=" + id + ", UserID=" + userId);

        await _tourLogService.LogAsync(id, userId, "view");

        return View(tour);
    }
}

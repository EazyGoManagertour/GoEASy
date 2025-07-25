using GoEASy.Models;
using System;
using System.Threading.Tasks;

public class TourLogService
{
    private readonly GoEasyContext _context;

    public TourLogService(GoEasyContext context)
    {
        _context = context;
    }

    public async Task LogAsync(int tourId, int? userId, string actionType)
    {
        var log = new TourViewLog
        {
            TourID = tourId,
            UserID = userId,
            ViewTime = DateTime.Now,
            ActionType = actionType
        };

        _context.TourViewLogs.Add(log);
        await _context.SaveChangesAsync(); // Cần thiết để lưu vào DB
    }
}
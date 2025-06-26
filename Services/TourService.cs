using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoEASy.Repositories;
using Microsoft.EntityFrameworkCore;

public class TourService
{
    private readonly GoEasyContext _context;

    public TourService(GoEasyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tour>> GetAllToursAsync()
    {
        return await _context.Tours
            .Include(t => t.TourImages)
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .ToListAsync();
    }

    public async Task<Tour> GetTourByIdAsync(int id)
    {
        return await _context.Tours
            .Include(t => t.TourImages)
            .Include(t => t.Destination)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TourId == id);
    }

    public async Task AddTourAsync(Tour tour)
    {
        _context.Tours.Add(tour);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTourAsync(Tour tour)
    {
        _context.Tours.Update(tour);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTourAsync(int id)
    {
        var tour = await _context.Tours.FindAsync(id);
        if (tour != null)
        {
            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
        }
    }
}

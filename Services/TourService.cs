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
<<<<<<< HEAD
            .Include(t => t.Reviews)
=======
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
            .ToListAsync();
    }

    public async Task<Tour> GetTourByIdAsync(int id)
    {
        return await _context.Tours
            .Include(t => t.TourImages)
<<<<<<< HEAD
=======
            .Include(t => t.Destination)
            .Include(t => t.Category)
>>>>>>> 33b4a60fd6af242bb2c1562a2cbee06a7b57d2bb
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

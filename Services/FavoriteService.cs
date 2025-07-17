using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly GoEasyContext _context;
        public FavoriteService(GoEasyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Favorite>> GetAllFavoritesAsync()
        {
            return await _context.Favorites
                .Include(f => f.Tour)
                .Include(f => f.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(int userId)
        {
            return await _context.Favorites
                .Include(f => f.Tour)
                .Include(f => f.User)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> RemoveFavoriteAsync(int userId, int tourId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.TourId == tourId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> AddFavoriteAsync(int userId, int tourId)
        {
            var exists = await _context.Favorites.AnyAsync(f => f.UserId == userId && f.TourId == tourId);
            if (exists)
                return false;

            var favorite = new Favorite
            {
                UserId = userId,
                TourId = tourId,
                CreatedAt = DateTime.Now
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
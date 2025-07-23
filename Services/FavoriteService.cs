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

        public async Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(int userID)
        {
            return await _context.Favorites
                .Include(f => f.Tour)
                .Include(f => f.User)
                .Where(f => f.UserID == userID)
                .ToListAsync();
        }

        public async Task<bool> RemoveFavoriteAsync(int userID, int tourID)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserID == userID && f.TourID == tourID);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> AddFavoriteAsync(int userID, int tourID)
        {
            var exists = await _context.Favorites.AnyAsync(f => f.UserID == userID && f.TourID == tourID);
            if (exists)
                return false;

            var favorite = new Favorite
            {
                UserID = userID,
                TourID = tourID,
                CreatedAt = DateTime.Now
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
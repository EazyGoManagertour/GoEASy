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
    }
} 
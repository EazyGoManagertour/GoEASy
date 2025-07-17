using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IFavoriteService
    {
        Task<IEnumerable<Favorite>> GetAllFavoritesAsync();
        Task<IEnumerable<Favorite>> GetFavoritesByUserIdAsync(int userId);
        Task<bool> RemoveFavoriteAsync(int userId, int tourId);
        Task<bool> AddFavoriteAsync(int userId, int tourId);
    }
} 
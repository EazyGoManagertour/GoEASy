using GoEASy.Models;
using GoEASy.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface ICategoryService : IGenericRepository<TourCategory>
    {
        Task<List<TourCategory>> GetAllCategoriesAsync();
        Task<List<TourCategory>> GetPagedCategoriesAsync(int page, int pageSize);
        Task<int> GetTotalPagesAsync(int pageSize);
        Task<TourCategory?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(TourCategory category);
        Task UpdateCategoryAsync(TourCategory category);
        Task DeleteCategoryAsync(TourCategory category);
        Task ToggleStatusAsync(int id);
    }
}

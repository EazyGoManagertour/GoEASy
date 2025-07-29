using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IDiscountService
    {
        Task<IEnumerable<Discount>> GetAllDiscountsAsync();
        Task<IEnumerable<Discount>> GetPagedDiscountsAsync(int page, int pageSize);
        Task<int> GetTotalPagesAsync(int pageSize);
        Task<Discount> GetDiscountByIdAsync(int id);
        Task<Discount> GetDiscountByCodeAsync(string code);
        Task CreateDiscountAsync(Discount discount);
        Task UpdateDiscountAsync(Discount discount);
        Task ToggleStatusAsync(int id);
    }
}
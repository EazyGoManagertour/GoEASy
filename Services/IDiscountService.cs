using GoEASy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public interface IDiscountService
    {
        Task<IEnumerable<Discount>> GetAllDiscountsAsync();
        Task<Discount> GetDiscountByIdAsync(int id);
        Task CreateDiscountAsync(Discount discount);
        Task UpdateDiscountAsync(Discount discount);
        Task ToggleStatusAsync(int id);
    }
}
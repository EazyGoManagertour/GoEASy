using GoEASy.Models;
using GoEASy.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GoEASy.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IGenericRepository<Discount> _discountRepository;

        public DiscountService(IGenericRepository<Discount> discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
        {
            return await _discountRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Discount>> GetPagedDiscountsAsync(int page, int pageSize)
        {
            var allDiscounts = await _discountRepository.GetAllAsync();
            return allDiscounts
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<int> GetTotalPagesAsync(int pageSize)
        {
            var allDiscounts = await _discountRepository.GetAllAsync();
            var totalDiscounts = allDiscounts.Count();
            return (int)Math.Ceiling((double)totalDiscounts / pageSize);
        }

        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            return await _discountRepository.GetByIdAsync(id);
        }

        public async Task<Discount> GetDiscountByCodeAsync(string code)
        {
            var allDiscounts = await _discountRepository.GetAllAsync();
            return allDiscounts.FirstOrDefault(d => d.Code == code);
        }

        public async Task CreateDiscountAsync(Discount discount)
        {
            await _discountRepository.AddAsync(discount);
            await _discountRepository.SaveAsync();
        }

        public async Task UpdateDiscountAsync(Discount discount)
        {
            _discountRepository.Update(discount);
            await _discountRepository.SaveAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            if (discount != null)
            {
                discount.Status = !discount.Status;
                discount.CreatedAt = DateTime.Now;
                _discountRepository.Update(discount);
                await _discountRepository.SaveAsync();
            }
        }
    }
}
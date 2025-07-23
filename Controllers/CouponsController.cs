using GoEASy.Filters;
using GoEASy.Models;
using GoEASy.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GoEASy.Controllers
{
    [AdminAuthorize]
    [Route("admin/coupons")]
    public class CouponsController : Controller
    {
        private readonly IDiscountService _discountService;

        public CouponsController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        // GET: admin/coupons
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            return View("~/Views/admin/coupons/Coupons.cshtml", discounts);
        }

        // POST: admin/coupons/create
        [HttpPost("create")]
        public async Task<IActionResult> Create(Discount discount)
        {
            var (isValid, errors) = ValidateDiscount(discount);

            if (!isValid)
            {
                TempData["Error"] = string.Join("<br/>", errors);
                return RedirectToAction("Index");
            }

            discount.CreatedAt = DateTime.Now;
            discount.Status = discount.Status ?? true;

            await _discountService.CreateDiscountAsync(discount);
            TempData["Success"] = "Coupon created successfully!";
            return RedirectToAction("Index");
        }

        // POST: admin/coupons/update
        [HttpPost("update")]
        public async Task<IActionResult> Update(Discount discount)
        {
            try
            {
                var (isValid, errors) = ValidateDiscount(discount, isUpdate: true);

                if (!isValid || discount.DiscountID == 0)
                {
                    TempData["Error"] = string.Join("<br/>", errors);
                    return RedirectToAction("Index");
                }

                var existingDiscount = await _discountService.GetDiscountByIdAsync(discount.DiscountID);
                if (existingDiscount == null)
                {
                    TempData["Error"] = "Coupon not found!";
                    return RedirectToAction("Index");
                }

                // Update fields
                existingDiscount.Code = discount.Code;
                existingDiscount.Description = discount.Description;
                existingDiscount.Percentage = discount.Percentage;
                existingDiscount.MaxAmount = discount.MaxAmount;
                existingDiscount.MinTotalPrice = discount.MinTotalPrice;
                existingDiscount.StartDate = discount.StartDate;
                existingDiscount.EndDate = discount.EndDate;
                existingDiscount.Status = discount.Status;
                existingDiscount.CreatedAt = DateTime.Now;

                await _discountService.UpdateDiscountAsync(existingDiscount);
                TempData["Success"] = "Coupon updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update coupon: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("delete-confirm")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            if (discount == null)
            {
                TempData["Error"] = "Coupon not found!";
                return RedirectToAction("Index");
            }

            discount.Status = false;
            discount.CreatedAt = DateTime.Now;

            await _discountService.UpdateDiscountAsync(discount);
            TempData["Success"] = "Coupon disabled (soft deleted) successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            await _discountService.ToggleStatusAsync(id);
            return Ok();
        }

        private (bool isValid, string[] errors) ValidateDiscount(Discount discount, bool isUpdate = false)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(discount.Code))
                errors.Add("Code is required.");

            if (discount.Percentage.HasValue && (discount.Percentage < 0 || discount.Percentage > 100))
                errors.Add("Percentage must be between 0 and 100.");

            if (discount.MaxAmount.HasValue && discount.MaxAmount < 0)
                errors.Add("Max amount cannot be negative.");

            if (discount.MinTotalPrice.HasValue && discount.MinTotalPrice < 0)
                errors.Add("Minimum total price cannot be negative.");

            if (discount.StartDate.HasValue && discount.EndDate.HasValue && discount.EndDate < discount.StartDate)
                errors.Add("End date cannot be before start date.");

            return (errors.Count == 0, errors.ToArray());
        }
    }
}
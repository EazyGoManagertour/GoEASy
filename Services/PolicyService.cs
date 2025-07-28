using GoEASy.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GoEASy.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly GoEasyContext _context;

        public PolicyService(GoEasyContext context)
        {
            _context = context;
        }

        public async Task<List<TourPolicy>> GetTourPoliciesAsync(int tourId)
        {
            return await _context.TourPolicies
                .Where(p => p.TourID == tourId && p.IsActive == true)
                .ToListAsync();
        }

        public async Task<TourPolicy?> GetPolicyByTypeAsync(int tourId, string policyType)
        {
            return await _context.TourPolicies
                .FirstOrDefaultAsync(p => p.TourID == tourId && p.PolicyType == policyType && p.IsActive == true);
        }

        public async Task<bool> ValidateChildPolicyAsync(int tourId, int adultCount, int childCount)
        {
            var policy = await GetPolicyByTypeAsync(tourId, "ChildPolicy");
            if (policy == null) return true; // Không có chính sách thì cho phép

            try
            {
                var policyData = JsonSerializer.Deserialize<ChildPolicyData>(policy.PolicyValue ?? "{}");
                if (policyData == null) return true;

                // ✅ Logic validation cải tiến:
                
                // 1. Không cho phép đặt chỉ vé trẻ em
                if (childCount > 0 && adultCount == 0)
                {
                    return false;
                }

                // 2. Phải có ít nhất 1 người lớn khi có trẻ em
                if (childCount > 0 && adultCount < policyData.MinAdultsPerChild)
                {
                    return false;
                }

                // 3. Tỷ lệ trẻ em/người lớn hợp lệ
                if (adultCount > 0 && childCount > (adultCount * policyData.MaxChildrenPerAdult))
                {
                    return false;
                }

                // 4. Không cho phép đặt vé nếu tổng số người = 0
                if (adultCount == 0 && childCount == 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return true; // Lỗi parse JSON thì cho phép
            }
        }

        public async Task<decimal> CalculateCancellationRefundAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(b => b.BookingID == bookingId);

            if (booking?.Tour?.StartDate == null) return 0;

            var policy = await GetPolicyByTypeAsync(booking.TourID.Value, "CancellationPolicy");
            if (policy == null) return 0;

            try
            {
                var policyData = JsonSerializer.Deserialize<CancellationPolicyData>(policy.PolicyValue ?? "{}");
                if (policyData == null) return 0;

                var daysUntilStart = (booking.Tour.StartDate.Value - DateTime.Now).Days;
                if (daysUntilStart >= policyData.RefundDays)
                {
                    return (booking.TotalPrice ?? 0) * (policyData.RefundPercentage / 100m);
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }

        public async Task<CancellationPolicyData?> GetCancellationPolicyAsync(int tourId)
        {
            var policy = await GetPolicyByTypeAsync(tourId, "CancellationPolicy");
            if (policy == null) return null;

            try
            {
                return JsonSerializer.Deserialize<CancellationPolicyData>(policy.PolicyValue ?? "{}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CanCancelBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(b => b.BookingID == bookingId);

            if (booking?.Tour?.StartDate == null) return false;

            var daysUntilStart = (booking.Tour.StartDate.Value - DateTime.Now).Days;
            return daysUntilStart >= 5; // Mặc định 5 ngày
        }

        public async Task<string> GetPolicyMessageAsync(int tourId, string policyType)
        {
            var policy = await GetPolicyByTypeAsync(tourId, policyType);
            if (policy == null) return "";

            try
            {
                if (policyType == "ChildPolicy")
                {
                    var policyData = JsonSerializer.Deserialize<ChildPolicyData>(policy.PolicyValue ?? "{}");
                    return policyData?.Message ?? policy.PolicyDescription ?? "";
                }
                else if (policyType == "CancellationPolicy")
                {
                    var policyData = JsonSerializer.Deserialize<CancellationPolicyData>(policy.PolicyValue ?? "{}");
                    return policyData?.Message ?? policy.PolicyDescription ?? "";
                }
                else
                {
                    return policy.PolicyDescription ?? "";
                }
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                System.Diagnostics.Debug.WriteLine($"Error parsing policy JSON: {ex.Message}");
                return policy.PolicyDescription ?? "";
            }
        }
    }

}

public class ChildPolicyData
{
    public int MinAdultsPerChild { get; set; } = 1;
    public int MaxChildrenPerAdult { get; set; } = 3;
    public int ChildAgeLimit { get; set; } = 18;
    public string Message { get; set; } = "Phải có ít nhất 1 người lớn cho mỗi 3 trẻ em";
}

public class CancellationPolicyData
{
    public int RefundDays { get; set; } = 5;
    public decimal RefundPercentage { get; set; } = 100;
    public string Message { get; set; } = "Hủy trước 5 ngày được hoàn 100% tiền";
}



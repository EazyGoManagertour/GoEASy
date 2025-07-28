using GoEASy.Models;

namespace GoEASy.Services
{
    public interface IPolicyService
    {
        Task<List<TourPolicy>> GetTourPoliciesAsync(int tourId);
        Task<TourPolicy?> GetPolicyByTypeAsync(int tourId, string policyType);
        Task<bool> ValidateChildPolicyAsync(int tourId, int adultCount, int childCount);
        Task<decimal> CalculateCancellationRefundAsync(int bookingId);
        Task<bool> CanCancelBookingAsync(int bookingId);
        Task<string> GetPolicyMessageAsync(int tourId, string policyType);
        Task<CancellationPolicyData?> GetCancellationPolicyAsync(int tourId);
    }
} 
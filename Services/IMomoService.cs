public interface IMomoService
{
    Task<MomoCreatePaymentResponse> CreatePaymentAsync(OrderInfoModel model);
    MomoExecuteResponseModel PaymentExecuteAsync(Microsoft.AspNetCore.Http.IQueryCollection collection);
} 
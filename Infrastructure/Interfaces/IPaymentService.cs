public interface IPaymentService
{
    Task<Response<string>> CreatePaymentAsync(int orderId, EnumPaymentMethod method);
    Task<Response<string>> UpdateStatusAsync(int orderId, EnumPaymentStatus status);
    // Task<Response<string>> RefundAsync(int orderId);
}
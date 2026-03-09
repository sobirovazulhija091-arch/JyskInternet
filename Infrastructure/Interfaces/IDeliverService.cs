public interface IDeliveryService
{
    Task<Response<string>> CreateDeliveryAsync(int orderId, EnumDeliveryType type);
    Task<Response<string>> UpdateStatusAsync(int orderId, EnumDeliveryStatus status);
    Task<Response<Delivery>> GetByOrderIdAsync(int orderId);
}
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Orders.Requests;
using TheGentlemanLibrary.Application.Models.Orders.Responses;

namespace TheGentlemanLibrary.Application.Models.Orders.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<OrderResponseModel>> CreateOrderAsync(OrderRequestModel orderRequest, CancellationToken cancellationToken);
        Task<ApiResponse<bool>> EditOrderAsync(OrderRequestModel orderRequest, CancellationToken cancellationToken);
        Task<ApiResponse<OrderResponseModel>> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<OrderResponseModel>>> GetOrdersAsync(CancellationToken ct);
        Task<ApiResponse<int>> GetOrdersCountAsync(CancellationToken ct);
        Task RemoveInactiveOrders(CancellationToken ct);
    }
}

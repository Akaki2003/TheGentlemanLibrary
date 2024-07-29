using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Application.Models.Orders.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> CreateOrderAsync(CreateOrderCommand orderRequest, CancellationToken ct);
        Task<bool> EditOrder(EditOrderCommand orderRequest, CancellationToken ct);
        Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<int> GetOrdersCountAsync(CancellationToken ct);
        Task RemoveInactiveOrders(CancellationToken ct);
    }
}

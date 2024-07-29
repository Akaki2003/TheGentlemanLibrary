using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Orders.Responses;

namespace TheGentlemanLibrary.Application.Models.Orders.Commands
{
    public record CreateOrderCommand(int Id, int UserId, int BookId, decimal Price) : IRequest<ApiResponse<OrderResponseModel>>;
}

using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Orders.Commands
{
    public record EditOrderCommand(int Id, int UserId, int BookId, decimal Price) : IRequest<ApiResponse<bool>>;
}

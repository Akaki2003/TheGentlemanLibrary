using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Orders.Responses;

namespace TheGentlemanLibrary.Application.Models.Orders.Queries
{
    public record GetOrdersQuery() : IRequest<ApiResponse<IEnumerable<OrderResponseModel>>>;
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Queries;
using TheGentlemanLibrary.Application.Models.Orders.Responses;

namespace TheGentlemanLibrary.API.Endpoints
{
    public static class OrdersEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/orders").WithTags("Orders").RequireAuthorization();
            group.MapGet("", GetOrders).WithName(nameof(GetOrders)).WithDescription("Get all orders");
            group.MapGet("{id}", GetOrderById).WithName(nameof(GetOrderById)).WithDescription("Get an order by its ID");
            group.MapPost("", CreateOrder).WithName(nameof(CreateOrder)).WithDescription("Create a new order");
        }

        public static async Task<ApiResponse<IEnumerable<OrderResponseModel>>> GetOrders(IMediator _mediator, [AsParameters] GetOrdersQuery query)
                     => await _mediator.Send(query);

        public static async Task<ApiResponse<OrderResponseModel>> GetOrderById(IMediator _mediator, [AsParameters] GetOrderByIdQuery query)
                     => await _mediator.Send(query);

        public static async Task<ApiResponse<OrderResponseModel>> CreateOrder(IMediator _mediator, [FromBody] CreateOrderCommand command)
                     => await _mediator.Send(command);
    }
}
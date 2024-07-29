using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Application.Models.Orders.Queries;
using TheGentlemanLibrary.Application.Models.Orders.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Orders.Handlers
{
    public class GetOrdersQueryHandler(IOrderRepository orderRepository, ILogger<GetOrdersQueryHandler> logger) : IRequestHandler<GetOrdersQuery, ApiResponse<IEnumerable<OrderResponseModel>>>
    {
        private readonly IOrderRepository _orderRepo = orderRepository;
        private readonly ILogger<GetOrdersQueryHandler> _logger = logger;

        public async Task<ApiResponse<IEnumerable<OrderResponseModel>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var orders = (await _orderRepo.GetOrdersAsync()).ToList();
                var responseModels = orders.Select(order => new OrderResponseModel
                {
                    UserId = order.UserId,
                    BookId = order.BookId,
                    Price = order.Price
                });
                return ApiResponse<IEnumerable<OrderResponseModel>>.Success(responseModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders");
                return ApiResponse<IEnumerable<OrderResponseModel>>.Fail(null, new List<string> { RsStrings.OrdersFetchError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

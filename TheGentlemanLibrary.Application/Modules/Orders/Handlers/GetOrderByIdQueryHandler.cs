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
    public class GetOrderByIdQueryHandler(IOrderRepository orderRepository, ILogger<GetOrderByIdQueryHandler> logger) : IRequestHandler<GetOrderByIdQuery, ApiResponse<OrderResponseModel>>
    {
        private readonly IOrderRepository _orderRepo = orderRepository;
        private readonly ILogger<GetOrderByIdQueryHandler> _logger;

        public async Task<ApiResponse<OrderResponseModel>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepo.GetOrderByIdAsync(request.ID, cancellationToken);
                if (order == null)
                {
                    return ApiResponse<OrderResponseModel>.Fail(null, new List<string> { RsStrings.OrderNotFound }, (int)HttpStatusCode.NotFound);
                }
                var responseModel = new OrderResponseModel
                {
                    UserId = order.UserId,
                    BookId = order.BookId,
                    Price = order.Price
                };
                return ApiResponse<OrderResponseModel>.Success(responseModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the order");
                return ApiResponse<OrderResponseModel>.Fail(null, new List<string> { RsStrings.OrderFetchError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}

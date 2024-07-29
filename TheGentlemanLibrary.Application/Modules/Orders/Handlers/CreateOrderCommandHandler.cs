using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Application.Models.Orders.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;
using FluentValidation;
using TheGentlemanLibrary.Application.Models.Books.Commands;

namespace TheGentlemanLibrary.Application.Models.Orders.Handlers
{
    public class CreateOrderCommandHandler(IOrderRepository orderRepository, ILogger<CreateOrderCommandHandler> logger) : IRequestHandler<CreateOrderCommand, ApiResponse<OrderResponseModel>>
    {
        private readonly IOrderRepository _orderRepo = orderRepository;
        private readonly ILogger<CreateOrderCommandHandler> _logger = logger;

        public async Task<ApiResponse<OrderResponseModel>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdOrder = await _orderRepo.CreateOrderAsync(request, cancellationToken);
                if (createdOrder == null)
                {
                    return ApiResponse<OrderResponseModel>.Fail(null, new List<string> { RsStrings.OrderCreationError }, (int)HttpStatusCode.BadRequest);
                }
                var responseModel = new OrderResponseModel
                {
                    UserId = createdOrder.UserId,
                    BookId = createdOrder.BookId,
                    Price = createdOrder.Price
                };
                return ApiResponse<OrderResponseModel>.Success(responseModel, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order");
                return ApiResponse<OrderResponseModel>.Fail(null, new List<string> { RsStrings.OrderCreationError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("Book Price must be greater than zero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User Id must be greater than zero.");

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book Id must be greater than zero.");
        }
    }
}

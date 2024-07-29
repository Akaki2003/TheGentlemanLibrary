using MediatR;
using System.Net;
using Microsoft.Extensions.Logging;
using TheGentlemanLibrary.Common.Resources;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Application.Models.BaseModels;
using FluentValidation;

namespace TheGentlemanLibrary.Application.Models.Orders.Handlers
{
    public class EditOrderCommandHandler(IOrderRepository orderRepository, ILogger<EditOrderCommandHandler> logger) : IRequestHandler<EditOrderCommand, ApiResponse<bool>>
    {
        private readonly IOrderRepository _orderRepo = orderRepository;
        private readonly ILogger<EditOrderCommandHandler> _logger = logger;

        public async Task<ApiResponse<bool>> Handle(EditOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderRepo.EditOrder(request, cancellationToken);
                if (result)
                {
                    return ApiResponse<bool>.Success(true);
                }
                else
                {
                    return ApiResponse<bool>.Fail(false, new List<string> { RsStrings.OrderEditError }, (int)HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the order");
                return ApiResponse<bool>.Fail(false, new List<string> { RsStrings.OrderEditError }, (int)HttpStatusCode.InternalServerError);
            }
        }
    }

    public class EditOrderCommandValidator : AbstractValidator<EditOrderCommand>
    {
        public EditOrderCommandValidator()
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

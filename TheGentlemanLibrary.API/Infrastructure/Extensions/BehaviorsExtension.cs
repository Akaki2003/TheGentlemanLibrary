using MediatR;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Commands;
using TheGentlemanLibrary.Application.Models.Books.Responses;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Responses;
using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Application.Models.Users.JWT;
using TheGentlemanLibrary.Application.Validation;

namespace TheGentlemanLibrary.API.Infrastructure.Extensions
{
    public static class BehaviorsExtension
    {
        public static MediatRServiceConfiguration AddMyBehaviors(this MediatRServiceConfiguration mediatrServiceConfig)
        {
            mediatrServiceConfig.AddBehavior<IPipelineBehavior<CreateAuthorCommand, ApiResponse<AuthorResponseModel>>, ValidationBehavior<CreateAuthorCommand, AuthorResponseModel>>();
            mediatrServiceConfig.AddBehavior<IPipelineBehavior<EditAuthorCommand, ApiResponse<bool>>, ValidationBehavior<EditAuthorCommand, bool>>();
            mediatrServiceConfig.AddBehavior<IPipelineBehavior<CreateBookCommand, ApiResponse<BookResponseModel>>, ValidationBehavior<CreateBookCommand, BookResponseModel>>();
            mediatrServiceConfig.AddBehavior<IPipelineBehavior<EditBookCommand, ApiResponse<bool>>, ValidationBehavior<EditBookCommand, bool>>();
            mediatrServiceConfig.AddBehavior<IPipelineBehavior<CreateOrderCommand, ApiResponse<OrderResponseModel>>, ValidationBehavior<CreateOrderCommand, OrderResponseModel>>();
            mediatrServiceConfig.AddBehavior<IPipelineBehavior<EditOrderCommand, ApiResponse<bool>>, ValidationBehavior<EditOrderCommand, bool>>();
            return mediatrServiceConfig;
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheGentlemanLibrary.API.Endpoints.Common;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Commands;
using TheGentlemanLibrary.Application.Models.Books.Queries;
using TheGentlemanLibrary.Application.Models.Books.Responses;

namespace TheGentlemanLibrary.API.Endpoints
{
    public static class BooksEndpoints
    {
        public static void MapBookEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/books").WithTags("Books").RequireAuthorization();
            group.MapGet("", GetBooks).WithName(nameof(GetBooks)).WithDescription("Get Books").AllowAnonymous();
            group.MapGet("{id}", GetBookById).WithName(nameof(GetBookById)).WithDescription("Get Book By Id");
            group.MapPost("", CreateBook).WithName(nameof(CreateBook)).WithDescription("Create Book");
            group.MapPut("", EditBook).WithName(nameof(EditBook)).WithDescription("Edit Book");
        }

        public static async Task<ApiResponse<IEnumerable<BookResponseModel>>> GetBooks(IMediator _mediator, [AsParameters] GetBooksQuery query)
            => await _mediator.Send(query);

        public static async Task<ApiResponse<BookResponseModel>> GetBookById(IMediator _mediator, [AsParameters] GetBookByIdQuery query)
            => await _mediator.Send(query);

        public static async Task<ApiResponse<BookResponseModel>> CreateBook(IMediator mediator, HttpContext httpContext, [FromBody] CreateBookCommand command)
        {
            var userId = UserHelper.GetUserId(httpContext);
            if (!userId.HasValue)
            {
                return new ApiResponse<BookResponseModel>();
            }
            command.UserId = UserHelper.GetUserId(httpContext).Value;
            return await mediator.Send(command);
        }

        public static async Task<ApiResponse<bool>> EditBook(IMediator mediator, HttpContext httpContext, [FromBody] EditBookCommand command)
        {
            var userId = UserHelper.GetUserId(httpContext);
            if (!userId.HasValue)
            {
                return new ApiResponse<bool>();
            }
            command.UserId = UserHelper.GetUserId(httpContext).Value;
            return await mediator.Send(command);
        }
    }
}

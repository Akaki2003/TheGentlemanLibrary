using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Authors.Queries;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.API.Endpoints
{
    public static class AuthorsEndpoints
    {
        public static void MapAuthorEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/authors").WithTags("Authors").RequireAuthorization();
            group.MapGet("", GetAuthors).WithName(nameof(GetAuthors)).WithDescription("Get Authors");
            group.MapGet("{id}", GetAuthorById).WithName(nameof(GetAuthorById)).WithDescription("Get Author By Id");
            group.MapPost("", CreateAuthor).WithName(nameof(CreateAuthor)).WithDescription("Create Author");
            group.MapPut("", EditAuthor).WithName(nameof(EditAuthor)).WithDescription("Edit Author");
        }

        public static async Task<ApiResponse<List<AuthorResponseModel>>> GetAuthors(IMediator _mediator, [AsParameters] GetAuthorsQuery query)
                     => await _mediator.Send(query);

        public static async Task<ApiResponse<AuthorResponseModel>> GetAuthorById(IMediator _mediator, [AsParameters] GetAuthorByIdQuery query)
                     => await _mediator.Send(query);

        public static async Task<ApiResponse<AuthorResponseModel>> CreateAuthor(IMediator _mediator, [FromBody] CreateAuthorCommand command)
                     => await _mediator.Send(command);

        public static async Task<ApiResponse<bool>> EditAuthor(IMediator _mediator, [FromBody] EditAuthorCommand command)
                     => await _mediator.Send(command);
    }
}

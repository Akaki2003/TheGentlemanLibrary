using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TheGentlemanLibrary.API.Endpoints.Common;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Application.Models.Users.Queries;
using TheGentlemanLibrary.Application.Models.Users.Responses;

namespace TheGentlemanLibrary.API.Endpoints
{
    public static class UsersEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/users").WithTags("Users").RequireAuthorization();
            group.MapGet("", GetProfile).WithName(nameof(GetProfile));
            group.MapPost("login", Login).WithName(nameof(Login)).AllowAnonymous();
            group.MapPost("register", Register).WithName(nameof(Register)).AllowAnonymous();
        }

        public static async Task<ApiResponse<ProfileResponseModel>> GetProfile(IMediator _mediator, HttpContext httpContext)
        {
            var userId = UserHelper.GetUserId(httpContext);
            return userId.HasValue ? await _mediator.Send(new GetProfileQuery(userId.Value)) : new ApiResponse<ProfileResponseModel>();
        }

        public static async Task<IResult> Login(IMediator _mediator, [FromBody] LoginCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return result?.Token.IsNullOrEmpty() == false ? Results.Ok(result) : Results.Unauthorized();
        }

        public static async Task<IResult> Register(IMediator _mediator, [FromBody] RegisterCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return !result.Token.IsNullOrEmpty() ? Results.Ok(result) : Results.Unauthorized();
        }
    }
}
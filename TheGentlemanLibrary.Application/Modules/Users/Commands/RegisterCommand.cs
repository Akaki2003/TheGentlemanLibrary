using MediatR;
using TheGentlemanLibrary.Application.Models.Users.JWT;

namespace TheGentlemanLibrary.Application.Models.Users.Commands
{
    public record RegisterCommand(string Email, string Password) : IRequest<JWTModel>;
}

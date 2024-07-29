using MediatR;
using TheGentlemanLibrary.Application.Models.Users.JWT;

namespace TheGentlemanLibrary.Application.Models.Users.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<JWTModel>;
}

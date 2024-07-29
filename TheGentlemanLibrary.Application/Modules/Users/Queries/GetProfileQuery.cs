using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Users.Responses;

namespace TheGentlemanLibrary.Application.Models.Users.Queries
{
    public record GetProfileQuery(int ID) : IRequest<ApiResponse<ProfileResponseModel>>;
}

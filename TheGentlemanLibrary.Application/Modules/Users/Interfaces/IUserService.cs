using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Users.JWT;
using TheGentlemanLibrary.Application.Models.Users.Requests;
using TheGentlemanLibrary.Application.Models.Users.Responses;

namespace TheGentlemanLibrary.Application.Models.Users.Interfaces
{
    public interface IUserService
    {
        Task<JWTModel> CreateUserAsync(UserModel model);
        Task<ApiResponse<ProfileResponseModel>> GetProfile(int id, CancellationToken ct = default);
        Task<JWTModel> Login(UserModel model);
    }
}

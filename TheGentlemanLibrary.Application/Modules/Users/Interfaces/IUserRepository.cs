using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Application.Models.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(RegisterCommand model);
        Task<User?> GetProfile(int id);
        Task<User> Login(LoginCommand model);
    }
}

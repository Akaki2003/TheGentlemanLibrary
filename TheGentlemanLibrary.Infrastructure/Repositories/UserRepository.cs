using Microsoft.AspNetCore.Identity;
using TheGentlemanLibrary.Application.Models.Users.Commands;
using TheGentlemanLibrary.Application.Models.Users.Interfaces;
using TheGentlemanLibrary.Domain.Entities;
using TheGentlemanLibrary.Infrastructure.Data;

namespace TheGentlemanLibrary.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context, UserManager<User> userManager) : BaseRepository<User>(context), IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<User?> GetProfile(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        public async Task<User> Login(LoginCommand model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            user ??= await _userManager.FindByEmailAsync(model.Email);
            if (await _userManager.CheckPasswordAsync(user, model.Password)) return user;
            return null;
        }
        public async Task<User> CreateUserAsync(RegisterCommand model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) return user;
            return null;
        }
    }
}

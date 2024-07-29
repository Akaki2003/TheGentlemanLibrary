using TheGentlemanLibrary.Application.Models.Users;

namespace TheGentlemanLibrary.Application.Models.Users.Responses
{
    public class ProfileResponseModel : IUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}

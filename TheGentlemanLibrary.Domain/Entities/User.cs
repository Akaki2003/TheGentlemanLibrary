using Microsoft.AspNetCore.Identity;

namespace TheGentlemanLibrary.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Book>? Books { get; set; }
    }


    public class UserRole : IdentityRole<int>
    {
    }
}

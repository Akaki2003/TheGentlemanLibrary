using TheGentlemanLibrary.Domain.Abstraction;

namespace TheGentlemanLibrary.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }

        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}

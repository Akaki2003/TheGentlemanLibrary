using TheGentlemanLibrary.Domain.Abstraction;

namespace TheGentlemanLibrary.Domain.Entities
{
    public class Book : BaseEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Pages { get; set; }
        public int AuthorId { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string? DateRange { get; set; }


        public Author? Author { get; set; }
        public User? User { get; set; }
    }
}

using TheGentlemanLibrary.Domain.Abstraction;

namespace TheGentlemanLibrary.Domain.Entities
{
    public class Author : BaseEntity
    {
        public int Id { get; set; }
        public string Country { get; set; } 
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public string? DateRange { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}

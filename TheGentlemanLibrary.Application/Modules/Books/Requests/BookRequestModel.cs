namespace TheGentlemanLibrary.Application.Models.Books.Requests
{
    public class BookRequestModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Pages { get; set; }
        public int AuthorId { get; set; }
        public int UserId { get; set; }
        public string? DateRange { get; set; }
    }
}

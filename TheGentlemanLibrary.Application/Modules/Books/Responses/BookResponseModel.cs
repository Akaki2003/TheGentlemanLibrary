namespace TheGentlemanLibrary.Application.Models.Books.Responses
{
    public class BookResponseModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Pages { get; set; }
        public int AuthorId { get; set; }
        public int UserId { get; set; }
        public string? DateRange { get; set; }
    }
}

namespace TheGentlemanLibrary.Application.Models.Authors.Responses
{
    public class AuthorResponseModel
    {
        public int Id { get; set; }
        public string Country { get; set; } 
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public string? DateRange { get; set; }
    }
}

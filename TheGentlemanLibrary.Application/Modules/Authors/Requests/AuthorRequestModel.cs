namespace TheGentlemanLibrary.Application.Models.Authors.Requests
{
    public class AuthorRequestModel
    {
        public int Id { get; set; }
        public DateTime? Born { get; set; }
        public string Country { get; set; }
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public string? DateRange { get; set; }
    }
}

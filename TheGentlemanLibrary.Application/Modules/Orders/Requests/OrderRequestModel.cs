namespace TheGentlemanLibrary.Application.Models.Orders.Requests
{
    public class OrderRequestModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public decimal Price { get; set; }
    }
}

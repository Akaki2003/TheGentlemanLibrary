using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Books.Commands
{
    public record EditBookCommand(int Id, string? Title, int? Pages, int AuthorId, string? DateRange) : IRequest<ApiResponse<bool>>
    {
        public int UserId { get; set; }
    };
}

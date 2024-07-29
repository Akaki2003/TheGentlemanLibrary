using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Responses;

namespace TheGentlemanLibrary.Application.Models.Books.Commands
{
    public record CreateBookCommand(int Id, string? Title, int? Pages, int AuthorId, string? DateRange) : IRequest<ApiResponse<BookResponseModel>>
    {
        public int UserId { get; set; }
    };
}

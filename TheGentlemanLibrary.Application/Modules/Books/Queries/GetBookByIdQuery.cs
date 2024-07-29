using MediatR;
using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Responses;

namespace TheGentlemanLibrary.Application.Models.Books.Queries
{
    public record GetBookByIdQuery(int ID) : IRequest<ApiResponse<BookResponseModel>>;
}

using TheGentlemanLibrary.Application.Models.BaseModels;
using TheGentlemanLibrary.Application.Models.Books.Requests;
using TheGentlemanLibrary.Application.Models.Books.Responses;

namespace TheGentlemanLibrary.Application.Models.Books.Interfaces
{
    public interface IBookService
    {
        Task<ApiResponse<BookResponseModel>> CreateBookAsync(BookRequestModel bookModel, CancellationToken cancellationToken);
        Task<ApiResponse<bool>> EditBookAsync(BookRequestModel bookModel, CancellationToken cancellationToken);
        Task<ApiResponse<BookResponseModel>> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<BookResponseModel>>> GetBooksAsync(CancellationToken ct = default);
        Task<ApiResponse<int>> GetBooksCountAsync();
        Task<ApiResponse<bool>> RemoveInactiveBooksAsync();
    }
}

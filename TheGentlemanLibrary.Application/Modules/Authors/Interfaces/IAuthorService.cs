using TheGentlemanLibrary.Application.Models.Authors.Requests;
using TheGentlemanLibrary.Application.Models.Authors.Responses;
using TheGentlemanLibrary.Application.Models.BaseModels;

namespace TheGentlemanLibrary.Application.Models.Authors.Interfaces
{
    public interface IAuthorService
    {
        Task<ApiResponse<AuthorResponseModel>> CreateAuthorAsync(AuthorRequestModel author, CancellationToken cancellationToken);
        Task<ApiResponse<bool>> EditAuthorAsync(AuthorRequestModel author, CancellationToken cancellationToken);
        Task<ApiResponse<AuthorResponseModel>> GetAuthorByIdAsync(int id, CancellationToken cancellationToken);
        Task<ApiResponse<List<AuthorResponseModel>>> GetAuthorsAsync(CancellationToken ct = default);
        Task<ApiResponse<int>> GetAuthorsCountAsync();
        Task<ApiResponse<bool>> RemoveInactiveAuthorsAsync();
    }
}

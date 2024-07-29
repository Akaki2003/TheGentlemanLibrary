using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Application.Models.Authors.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author?> CreateAuthorAsync(CreateAuthorCommand authorRequest);
        Task<bool> EditAuthor(EditAuthorCommand authorRequest);
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<int> GetAuthorsCountAsync();
        Task RemoveInactiveAuthors();
    }
}

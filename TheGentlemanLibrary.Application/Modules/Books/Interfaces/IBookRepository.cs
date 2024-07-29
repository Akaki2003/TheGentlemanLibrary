using TheGentlemanLibrary.Domain.Entities;

namespace TheGentlemanLibrary.Application.Models.Books.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> CreateBookAsync(Book book, CancellationToken cancellationToken);
        Task<bool> EditBook(Book book, CancellationToken cancellationToken);
        Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<int> GetBooksCountAsync();
        Task RemoveInactiveBooks();
    }
}

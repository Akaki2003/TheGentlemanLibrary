using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using TheGentlemanLibrary.Application.Models.Books.Interfaces;
using TheGentlemanLibrary.Domain.Entities;
using TheGentlemanLibrary.Infrastructure.Data;

namespace TheGentlemanLibrary.Infrastructure.Repositories
{
    public class BookRepository(IOptions<ConnectionStrings> conn) : IBookRepository
    {
        private readonly string _connectionString = conn.Value.DefaultConnection;
        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            const string sql = @"
                SELECT b.*, a.*, u.*
                FROM ""Books"" b
                LEFT JOIN ""Authors"" a ON b.""AuthorId"" = a.""Id""
                LEFT JOIN ""Users"" u ON b.""UserId"" = u.""Id""
                ORDER BY b.""Id"" DESC";

            return await Connection.QueryAsync<Book, Author, User, Book>(
                sql,
                (book, author, user) =>
                {
                    book.Author = author;
                    book.User = user;
                    return book;
                },
                splitOn: "Id,Id");
        }

        public async Task<int> GetBooksCountAsync()
        {
            const string sql = @"SELECT COUNT(*) FROM ""Books""";
            return await Connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<Book?> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT b.*, a.*, u.*
                FROM ""Books"" b
                LEFT JOIN ""Authors"" a ON b.""AuthorId"" = a.""Id""
                LEFT JOIN ""Users"" u ON b.""UserId"" = u.""Id""
                WHERE b.""Id"" = @Id";

            var result = await Connection.QueryAsync<Book, Author, User, Book>(
                sql,
                (book, author, user) =>
                {
                    book.Author = author;
                    book.User = user;
                    return book;
                },
                new { Id = id },
                splitOn: "Id,Id");

            return result.FirstOrDefault();
        }

        public async Task<Book?> CreateBookAsync(Book book, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO ""Books"" (""Title"", ""Pages"", ""AuthorId"", ""UserId"", ""Price"", ""DateRange"", ""CreatedAt"")
                VALUES (@Title, @Pages, @AuthorId, @UserId, @Price, @DateRange, @CreatedAt)
                RETURNING ""Id"", ""Title"", ""Pages"", ""AuthorId"", ""UserId"", ""Price"", ""DateRange"", ""CreatedAt""";
            var parameters = new
            {
                book.Title,
                book.Pages,
                book.AuthorId,
                book.UserId,
                book.Price,
                book.DateRange,
                CreatedAt = DateTime.UtcNow
            };
            return await Connection.QuerySingleAsync<Book>(sql, parameters);
        }

        public async Task<bool> EditBook(Book book, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE ""Books""
                SET ""Title"" = @Title, ""Pages"" = @Pages, ""AuthorId"" = @AuthorId, 
                    ""UserId"" = @UserId, ""Price"" = @Price, ""DateRange"" = @DateRange,
                    ""ModifiedAt"" = @ModifiedAt
                WHERE ""Id"" = @Id";
            var parameters = new
            {
                book.Id,
                book.Title,
                book.Pages,
                book.AuthorId,
                book.UserId,
                book.Price,
                book.DateRange,
                ModifiedAt = DateTime.UtcNow
            };
            int affectedRows = await Connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task RemoveInactiveBooks()
        {
            const string sql = @"
            UPDATE ""Books""
            SET ""DeletedAt"" = @DeletedAt
            WHERE (CURRENT_DATE - ""CreatedAt""::date) > 365 AND ""DeletedAt"" IS NULL";
            await Connection.ExecuteAsync(sql, new { DeletedAt = DateTime.UtcNow });
        }
    }
}
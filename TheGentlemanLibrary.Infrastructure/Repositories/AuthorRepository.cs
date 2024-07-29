using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Authors.Interfaces;
using TheGentlemanLibrary.Domain.Entities;
using TheGentlemanLibrary.Infrastructure.Data;

namespace TheGentlemanLibrary.Infrastructure.Repositories
{
    public class AuthorRepository(IOptions<ConnectionStrings> conn) : IAuthorRepository
    {
        private readonly string _connectionString = conn.Value.DefaultConnection;
        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            const string sql = @"SELECT * FROM ""Authors"" a ORDER BY ""Id"" DESC";
            return await Connection.QueryAsync<Author>(sql);
        }

        public async Task<int> GetAuthorsCountAsync()
        {
            const string sql = @"SELECT COUNT(*) FROM ""Authors""";
            return await Connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM ""Authors"" WHERE ""Id"" = @Id";
            return await Connection.QueryFirstOrDefaultAsync<Author>(sql, new { Id = id });
        }

        public async Task<Author?> CreateAuthorAsync(CreateAuthorCommand authorRequest)
        {
            const string sql = @"
                INSERT INTO ""Authors"" (""Name"", ""Biography"", ""Country"", ""DateRange"", ""CreatedAt"")
                VALUES (@Name, @Biography, @Country, @DateRange, @CreatedAt)
                RETURNING ""Id"", ""Name"", ""Biography"", ""Country"", ""DateRange"", ""CreatedAt""";
            var parameters = new
            {
                authorRequest.Name,
                authorRequest.Biography,
                authorRequest.Country,
                authorRequest.DateRange,
                CreatedAt = DateTime.UtcNow
            };
            return await Connection.QuerySingleAsync<Author>(sql, parameters);
        }

        public async Task<bool> EditAuthor(EditAuthorCommand authorRequest)
        {
            const string sql = @"
                UPDATE ""Authors""
                SET ""Name"" = @Name, ""Biography"" = @Biography, ""Country"" = @Country, 
                    ""DateRange"" = @DateRange, ""ModifiedAt"" = @ModifiedAt
                WHERE ""Id"" = @Id";
            var parameters = new
            {
                authorRequest.Id,
                authorRequest.Name,
                authorRequest.Biography,
                authorRequest.Country,
                authorRequest.DateRange,
                ModifiedAt = DateTime.UtcNow
            };
            int affectedRows = await Connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task RemoveInactiveAuthors()
        {
            const string sql = @"
            UPDATE ""Authors""
            SET ""DeletedAt"" = @DeletedAt
            WHERE (CURRENT_DATE - ""CreatedAt""::date) > 365 AND ""DeletedAt"" IS NULL";
            await Connection.ExecuteAsync(sql, new { DeletedAt = DateTime.UtcNow });
        }
    }
}
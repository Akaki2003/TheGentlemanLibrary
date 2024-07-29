using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Interfaces;
using TheGentlemanLibrary.Domain.Entities;
using TheGentlemanLibrary.Infrastructure.Data;

namespace TheGentlemanLibrary.Infrastructure.Repositories
{
    public class OrderRepository(IOptions<ConnectionStrings> conn) : IOrderRepository
    {
        private readonly string _connectionString = conn.Value.DefaultConnection;
        private IDbConnection Connection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            const string sql = @"
                SELECT o.*, u.*, b.*
                FROM ""Orders"" o
                LEFT JOIN ""Users"" u ON o.""UserId"" = u.""Id""
                LEFT JOIN ""Books"" b ON o.""BookId"" = b.""Id""
                ORDER BY o.""Id"" DESC";

            return await Connection.QueryAsync<Order, User, Book, Order>(
                sql,
                (order, user, book) =>
                {
                    order.User = user;
                    order.Book = book;
                    return order;
                },
                splitOn: "Id,Id");
        }

        public async Task<int> GetOrdersCountAsync(CancellationToken ct)
        {
            const string sql = @"SELECT COUNT(*) FROM ""Orders""";
            return await Connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken ct)
        {
            const string sql = @"
                SELECT o.*, u.*, b.*
                FROM ""Orders"" o
                LEFT JOIN ""Users"" u ON o.""UserId"" = u.""Id""
                LEFT JOIN ""Books"" b ON o.""BookId"" = b.""Id""
                WHERE o.""Id"" = @Id";

            var result = await Connection.QueryAsync<Order, User, Book, Order>(
                sql,
                (order, user, book) =>
                {
                    order.User = user;
                    order.Book = book;
                    return order;
                },
                new { Id = id },
                splitOn: "Id,Id");

            return result.FirstOrDefault();
        }

        public async Task<Order?> CreateOrderAsync(CreateOrderCommand orderRequest, CancellationToken ct)
        {
            const string sql = @"
                INSERT INTO ""Orders"" (""UserId"", ""BookId"", ""Price"", ""CreatedAt"")
                VALUES (@UserId, @BookId, @Price, @CreatedAt)
                RETURNING ""Id"", ""UserId"", ""BookId"", ""Price"", ""CreatedAt""";

            var parameters = new
            {
                orderRequest.UserId,
                orderRequest.BookId,
                orderRequest.Price,
                CreatedAt = DateTime.UtcNow
            };

            return await Connection.QuerySingleOrDefaultAsync<Order>(sql, parameters);
        }

        public async Task<bool> EditOrder(EditOrderCommand orderRequest, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE ""Orders""
                SET ""UserId"" = @UserId, ""BookId"" = @BookId, ""Price"" = @Price, 
                    ""ModifiedAt"" = @ModifiedAt
                WHERE ""Id"" = @Id";

            var parameters = new
            {
                orderRequest.Id,
                orderRequest.UserId,
                orderRequest.BookId,
                orderRequest.Price,
                ModifiedAt = DateTime.UtcNow
            };

            int affectedRows = await Connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task RemoveInactiveOrders(CancellationToken ct)
        {
            const string sql = @"
            UPDATE ""Orders""
            SET ""DeletedAt"" = @DeletedAt
            WHERE (CURRENT_DATE - ""CreatedAt""::date) > 365 AND ""DeletedAt"" IS NULL";

            await Connection.ExecuteAsync(sql, new { DeletedAt = DateTime.UtcNow });
        }
    }
}
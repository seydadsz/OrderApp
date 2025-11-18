using Microsoft.Data.SqlClient;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string? _connectionString;

        public OrderRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<OrderWithDetails>> GetAllWithDetailsAsync()
        {
            var list = new List<OrderWithDetails>();

            string sql = @"
                SELECT o.Id, o.OrderNumber, o.CustomerId, o.ProductId,
                       o.Quantity, o.UnitPrice, o.IsConfirmed, o.CreatedAt,
                       c.CustomerName, p.StockName
                FROM Orders o
                JOIN Customers c ON o.CustomerId = c.Id
                JOIN Products p ON o.ProductId = p.Id
                ORDER BY o.CreatedAt DESC";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);
            await con.OpenAsync();

            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new OrderWithDetails
                {
                    Id = reader.GetInt32(0),
                    OrderNumber = reader.GetString(1),
                    CustomerId = reader.GetInt32(2),
                    ProductId = reader.GetInt32(3),
                    Quantity = reader.GetInt32(4),
                    UnitPrice = reader.GetDecimal(5),
                    IsConfirmed = reader.GetBoolean(6),
                    CreatedAt = reader.GetDateTime(7),
                    CustomerName = reader.GetString(8),
                    StockName = reader.GetString(9)
                });
            }

            return list;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            string sql = @"
                SELECT Id, OrderNumber, CustomerId, ProductId,
                       Quantity, UnitPrice, ConfirmationToken,
                       IsConfirmed, CreatedAt
                FROM Orders
                WHERE Id = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Order
                {
                    Id = reader.GetInt32(0),
                    OrderNumber = reader.GetString(1),
                    CustomerId = reader.GetInt32(2),
                    ProductId = reader.GetInt32(3),
                    Quantity = reader.GetInt32(4),
                    UnitPrice = reader.GetDecimal(5),
                    ConfirmationToken = reader.IsDBNull(6) ? null : reader.GetString(6),
                    IsConfirmed = reader.GetBoolean(7),
                    CreatedAt = reader.GetDateTime(8)
                };
            }

            return null;
        }

        public async Task AddAsync(Order order)
        {
            string sql = @"
                INSERT INTO Orders (OrderNumber, CustomerId, ProductId, Quantity, UnitPrice, ConfirmationToken, CreatedAt)
                VALUES (@num, @cid, @pid, @qty, @price, @token, @date)";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@num", order.OrderNumber);
            cmd.Parameters.AddWithValue("@cid", order.CustomerId);
            cmd.Parameters.AddWithValue("@pid", order.ProductId);
            cmd.Parameters.AddWithValue("@qty", order.Quantity);
            cmd.Parameters.AddWithValue("@price", order.UnitPrice);
            cmd.Parameters.AddWithValue("@token", (object?)order.ConfirmationToken ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@date", order.CreatedAt);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Order?> GetByTokenAsync(string token)
        {
            string sql = @"
                SELECT Id, OrderNumber, CustomerId, ProductId,
                       Quantity, UnitPrice, ConfirmationToken,
                       IsConfirmed, CreatedAt
                FROM Orders
                WHERE ConfirmationToken = @token";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@token", token);
            await con.OpenAsync();

            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Order
                {
                    Id = reader.GetInt32(0),
                    OrderNumber = reader.GetString(1),
                    CustomerId = reader.GetInt32(2),
                    ProductId = reader.GetInt32(3),
                    Quantity = reader.GetInt32(4),
                    UnitPrice = reader.GetDecimal(5),
                    ConfirmationToken = reader.IsDBNull(6) ? null : reader.GetString(6),
                    IsConfirmed = reader.GetBoolean(7),
                    CreatedAt = reader.GetDateTime(8)
                };
            }

            return null;
        }

        public async Task ConfirmAsync(int orderId)
        {
            string sql = "UPDATE Orders SET IsConfirmed = 1 WHERE Id = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", orderId);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}

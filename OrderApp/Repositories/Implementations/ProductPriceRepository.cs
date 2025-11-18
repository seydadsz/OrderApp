using Microsoft.Data.SqlClient;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories.Implementations
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly string? _connectionString;

        public ProductPriceRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        // TÜM fiyatları ürünle birlikte getirir (Index için)
        public async Task<List<ProductPriceWithProduct>> GetAllWithProductsAsync()
        {
            var list = new List<ProductPriceWithProduct>();

            string sql = @"
                SELECT p.Id, p.ProductId, p.Price, p.ValidFrom, pr.StockName
                FROM ProductPrices p
                INNER JOIN Products pr ON p.ProductId = pr.Id
                ORDER BY p.ValidFrom DESC";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ProductPriceWithProduct
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    Price = reader.GetDecimal(2),
                    ValidFrom = reader.GetDateTime(3),
                    StockName = reader.GetString(4)
                });
            }

            return list;
        }

        // 🔥 YENİ: Sadece bir ürüne ait fiyat geçmişi (History için)
        public async Task<List<ProductPriceWithProduct>> GetPricesByProductIdAsync(int productId)
        {
            var list = new List<ProductPriceWithProduct>();

            string sql = @"
                SELECT 
                    p.Id,
                    p.ProductId,
                    p.Price,
                    p.ValidFrom,
                    pr.StockName
                FROM ProductPrices p
                INNER JOIN Products pr ON p.ProductId = pr.Id
                WHERE p.ProductId = @pid
                ORDER BY p.ValidFrom DESC";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@pid", productId);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ProductPriceWithProduct
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    Price = reader.GetDecimal(2),
                    ValidFrom = reader.GetDateTime(3),
                    StockName = reader.GetString(4)
                });
            }

            return list;
        }

        // ID’ye göre fiyat getir
        public async Task<ProductPrice?> GetByIdAsync(int id)
        {
            string sql = @"
                SELECT Id, ProductId, Price, ValidFrom
                FROM ProductPrices
                WHERE Id = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", id);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ProductPrice
                {
                    Id = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1),
                    Price = reader.GetDecimal(2),
                    ValidFrom = reader.GetDateTime(3)
                };
            }

            return null;
        }

        // Yeni fiyat ekle
        public async Task AddAsync(ProductPrice price)
        {
            string sql = @"
                INSERT INTO ProductPrices (ProductId, Price, ValidFrom)
                VALUES (@pid, @price, @date)";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@pid", price.ProductId);
            cmd.Parameters.AddWithValue("@price", price.Price);
            cmd.Parameters.AddWithValue("@date", price.ValidFrom);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // Son fiyatı getir
        public async Task<decimal?> GetLatestPriceAsync(int productId)
        {
            string sql = @"
                SELECT TOP 1 Price
                FROM ProductPrices
                WHERE ProductId = @pid
                ORDER BY ValidFrom DESC";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@pid", productId);

            await con.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();
            return result != null && result != DBNull.Value
                ? Convert.ToDecimal(result)
                : null;
        }

        // Fiyat güncelle
        public async Task UpdateAsync(ProductPrice price)
        {
            string sql = @"
                UPDATE ProductPrices
                SET 
                    ProductId = @pid,
                    Price = @price,
                    ValidFrom = @date
                WHERE Id = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", price.Id);
            cmd.Parameters.AddWithValue("@pid", price.ProductId);
            cmd.Parameters.AddWithValue("@price", price.Price);
            cmd.Parameters.AddWithValue("@date", price.ValidFrom);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}

using Microsoft.Data.SqlClient;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly string? _connectionString;

        public ProductRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        
        //  TÜM ÜRÜNLERİ GETİR
        
        public async Task<List<Product>> GetAllAsync()
        {
            var list = new List<Product>();
            string sql = "SELECT Id, StockCode, StockName FROM Products";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    StockCode = reader.GetString(1),
                    StockName = reader.GetString(2)
                });
            }

            return list;
        }

     
        public async Task<Product?> GetByIdAsync(int id)
        {
            string sql = "SELECT Id, StockCode, StockName FROM Products WHERE Id = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Product
                {
                    Id = reader.GetInt32(0),
                    StockCode = reader.GetString(1),
                    StockName = reader.GetString(2)
                };
            }

            return null;
        }

 
        public async Task<bool> HasOrdersAsync(int productId)
        {
            string sql = "SELECT COUNT(*) FROM Orders WHERE ProductId = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", productId);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            int count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }


        public async Task<bool> StockCodeExistsAsync(string stockCode)
        {
            string sql = "SELECT COUNT(*) FROM Products WHERE StockCode = @code";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@code", stockCode);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            int count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }


        public async Task AddAsync(Product product)
        {
            string sql = @"
                INSERT INTO Products (StockCode, StockName)
                VALUES (@code, @name)";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@code", product.StockCode);
            cmd.Parameters.AddWithValue("@name", product.StockName);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }


        public async Task DeleteAsync(int id)
        {
            using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();

            // 1) Önce ürünün fiyat geçmişini sil
            string deletePricesSql = "DELETE FROM ProductPrices WHERE ProductId = @id";
            using (var cmdPrices = new SqlCommand(deletePricesSql, con))
            {
                cmdPrices.Parameters.AddWithValue("@id", id);
                await cmdPrices.ExecuteNonQueryAsync();
            }

            // 2) Sonra ürünü sil
            string deleteProductSql = "DELETE FROM Products WHERE Id = @id";
            using (var cmdProduct = new SqlCommand(deleteProductSql, con))
            {
                cmdProduct.Parameters.AddWithValue("@id", id);
                await cmdProduct.ExecuteNonQueryAsync();
            }
        }
    }
}

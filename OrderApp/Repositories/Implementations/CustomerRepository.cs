using Microsoft.Data.SqlClient;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string? _connectionString;

        public CustomerRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var list = new List<Customer>();
            string sql = "SELECT Id, CustomerCode, CustomerName, Email FROM Customers";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new Customer
                {
                    Id = reader.GetInt32(0),
                    CustomerCode = reader.GetString(1),
                    CustomerName = reader.GetString(2),
                    Email = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }

            return list;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            string sql = "SELECT Id, CustomerCode, CustomerName, Email FROM Customers WHERE Id = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", id);

            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Customer
                {
                    Id = reader.GetInt32(0),
                    CustomerCode = reader.GetString(1),
                    CustomerName = reader.GetString(2),
                    Email = reader.IsDBNull(3) ? null : reader.GetString(3)
                };
            }

            return null;
        }

        public async Task<bool> CustomerCodeExistsAsync(string code)
        {
            string sql = "SELECT COUNT(*) FROM Customers WHERE CustomerCode = @code";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@code", code);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            int count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }

        public async Task AddAsync(Customer customer)
        {
            string sql = @"
                INSERT INTO Customers (CustomerCode, CustomerName, Email) 
                VALUES (@code, @name, @mail)
            ";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@code", customer.CustomerCode);
            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
            cmd.Parameters.AddWithValue("@mail", (object?)customer.Email ?? DBNull.Value);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        // Güvenli silme (Siparişi olan müşteri silinemez)
        public async Task DeleteAsync(int id)
        {
            string sql = "DELETE FROM Customers WHERE Id = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                // SQL FK çakışması 
                if (ex.Message.Contains("FK_Orders_Customer"))
                {
                    throw new Exception("Bu müşteri siparişlerde kullanıldığı için silinemez.");
                }

                throw;
            }
        }

        //  Müşterinin siparişi var mı kontrolü
        public async Task<bool> HasOrdersAsync(int customerId)
        {
            string sql = "SELECT COUNT(*) FROM Orders WHERE CustomerId = @id";

            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", customerId);

            await con.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            int count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }
    }
}

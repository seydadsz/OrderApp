using OrderApp.Models;

namespace OrderApp.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<bool> StockCodeExistsAsync(string stockCode);
        Task AddAsync(Product product);
        Task<bool> HasOrdersAsync(int productId);
        Task DeleteAsync(int id);
    }
}

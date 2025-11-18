using OrderApp.Models;

namespace OrderApp.Repositories.Interfaces
{
    public interface IProductPriceRepository
    {
        Task<List<ProductPriceWithProduct>> GetAllWithProductsAsync();
        Task AddAsync(ProductPrice price);
        Task<decimal?> GetLatestPriceAsync(int productId);

        Task<ProductPrice?> GetByIdAsync(int id);
        Task UpdateAsync(ProductPrice price);

        Task<List<ProductPriceWithProduct>> GetPricesByProductIdAsync(int productId);
    }
}

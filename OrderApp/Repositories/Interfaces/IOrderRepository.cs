using OrderApp.Models;

namespace OrderApp.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderWithDetails>> GetAllWithDetailsAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task<Order?> GetByTokenAsync(string token);
        Task ConfirmAsync(int orderId);
    }
}

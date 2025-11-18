using OrderApp.Models;

namespace OrderApp.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<bool> CustomerCodeExistsAsync(string code);
        Task AddAsync(Customer customer);
        Task DeleteAsync(int id);
        Task<bool> HasOrdersAsync(int customerId);
    }
}

using ObucaDanilo.Models;

namespace ObucaDanilo.Repositories
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        public Task<Order?> GetByIdAsync(string id);
        public Task AddAsync(Order order);
        public Task UpdateAsync(Order order);
        public Task<bool> ExistsAsync(string id);
    }
}
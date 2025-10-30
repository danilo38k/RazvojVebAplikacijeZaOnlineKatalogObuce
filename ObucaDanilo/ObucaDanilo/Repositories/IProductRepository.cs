using ObucaDanilo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObucaDanilo.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId);
        Task<IEnumerable<Product>> GetByCategoryPagedAsync(string categoryId, int skip, int take);
        Task<IEnumerable<Product>> SearchAsync(string searchTerm);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<bool> ExistsAsync(string id);
    }
}
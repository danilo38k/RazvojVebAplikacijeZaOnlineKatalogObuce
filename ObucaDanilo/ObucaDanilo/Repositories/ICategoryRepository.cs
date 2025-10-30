using ObucaDanilo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObucaDanilo.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(string id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<bool> ExistsAsync(string id);


    }
}
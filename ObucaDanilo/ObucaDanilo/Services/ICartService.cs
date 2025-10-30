using ObucaDanilo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObucaDanilo.Services
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetCartItemsAsync(string userId);
        Task AddToCartAsync(string userId, string productId, int quantity = 1);
        Task UpdateCartItemAsync(string cartItemId, int quantity);
        Task RemoveFromCartAsync(string cartItemId);
        Task ClearCartAsync(string userId);
        Task<int> GetCartCountAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
    }
}
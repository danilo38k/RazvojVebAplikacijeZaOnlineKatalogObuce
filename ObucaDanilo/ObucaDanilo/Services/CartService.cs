using Microsoft.EntityFrameworkCore;
using ObucaDanilo.Data;
using ObucaDanilo.Models;

namespace ObucaDanilo.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        // konstruktor - injektuje kontekst baze
        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // vraća sve stavke u korpi za korisnika
        public async Task<IEnumerable<Cart>> GetCartItemsAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.AddedAt)
                .ToListAsync();
        }

        // dodaje proizvod u korpu ili povećava količinu ako već postoji
        public async Task AddToCartAsync(string userId, string productId, int quantity = 1)
        {
            var existingItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _context.Carts.Update(existingItem);
            }
            else
            {
                var cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _context.Carts.AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        // ažurira količinu za stavku u korpi
        public async Task UpdateCartItemAsync(string cartItemId, int quantity)
        {
            var cartItem = await _context.Carts.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _context.Carts.Update(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        // uklanja stavku iz korpe
        public async Task RemoveFromCartAsync(string cartItemId)
        {
            var cartItem = await _context.Carts.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        // prazni korpu korisnika
        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        // vraća ukupan broj proizvoda u korpi
        public async Task<int> GetCartCountAsync(string userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Quantity);
        }

        // vraća ukupnu cenu korpe
        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .SumAsync(c => c.Product.Price * c.Quantity);
        }
    }
}
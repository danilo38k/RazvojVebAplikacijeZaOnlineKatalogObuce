using Microsoft.EntityFrameworkCore;
using ObucaDanilo.Data;
using ObucaDanilo.Models;

namespace ObucaDanilo.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        // konstruktor - injektuje kontekst baze
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // vraća sve porudžbine korisnika
        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // vraća porudžbinu po id-u
        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // dodaje novu porudžbinu
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        // ažurira porudžbinu
        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        // proverava da li porudžbina postoji po id-u
        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }
    }
}
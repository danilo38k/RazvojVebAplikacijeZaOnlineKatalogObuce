using Microsoft.EntityFrameworkCore;
using ObucaDanilo.Data;
using ObucaDanilo.Models;

namespace ObucaDanilo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        // konstruktor - injektuje kontekst baze
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // vraća korisnika po id-u
        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        // vraća korisnika po email adresi
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // vraća sve korisnike
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // dodaje novog korisnika
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // ažurira korisnika
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // briše korisnika
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        // proverava da li korisnik postoji po id-u
        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }
    }
}
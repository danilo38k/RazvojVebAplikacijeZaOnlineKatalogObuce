using ObucaDanilo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObucaDanilo.Repositories
{
    public interface IUserRepository
    {
        // Vraća korisnika po ID-u
        Task<User?> GetByIdAsync(string id);

        // Vraća korisnika po email adresi
        Task<User?> GetByEmailAsync(string email);

        // Vraća sve korisnike
        Task<IEnumerable<User>> GetAllAsync();

        // Dodaje novog korisnika
        Task AddAsync(User user);

        // Ažurira korisnika
        Task UpdateAsync(User user);

        // Briše korisnika
        Task DeleteAsync(User user);

        // Proverava da li korisnik postoji po ID-u
        Task<bool> ExistsAsync(string id);
    }
}
using Microsoft.EntityFrameworkCore;
using ObucaDanilo.Data;
using ObucaDanilo.Models;

namespace ObucaDanilo.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        // konstruktor - injektuje kontekst baze
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // vraća sve kategorije
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        // vraća kategoriju po id-u
        public async Task<Category?> GetByIdAsync(string id)
        {
            return await _context.Categories.FindAsync(id);
        }

        // dodaje novu kategoriju
        public async Task AddAsync(Category category)
        {
            var maxOrder = await _context.Categories.MaxAsync(c => (int?)c.DisplayOrder) ?? 0;
            category.DisplayOrder = maxOrder + 1;

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        // ažurira kategoriju
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        // briše kategoriju
        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        // proverava da li kategorija postoji po id-u
        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }



    }
}
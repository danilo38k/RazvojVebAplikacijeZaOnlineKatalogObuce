using Microsoft.EntityFrameworkCore;
using ObucaDanilo.Data;
using ObucaDanilo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObucaDanilo.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // konstruktor - injektuje kontekst baze
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // vraća sve proizvode
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // vraća proizvod po id-u
        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // vraća proizvode po kategoriji
        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId)
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // vraća proizvode po strani (paginacija)
        public async Task<IEnumerable<Product>> GetByCategoryPagedAsync(string categoryId, int skip, int take)
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        // pretraga proizvoda po terminu
        public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Where(p =>
                    p.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm.ToLower())) ||
                    p.ProductCategories.Any(pc => pc.Category.Name.ToLower().Contains(searchTerm.ToLower()))
                )
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // dodaje novi proizvod
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        // ažurira proizvod
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // briše proizvod
        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        // proverava da li proizvod postoji po id-u
        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }
    }
}
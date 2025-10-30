using Microsoft.AspNetCore.Mvc;
using ObucaDanilo.Repositories;

namespace ShoeCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        // konstruktor - injektuje repozitorijume kategorija i proizvoda
        public HomeController(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // prikazuje početnu stranicu sa kategorijama
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        // pretraga proizvoda po terminu
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var products = await _productRepository.SearchAsync(searchTerm);
            return View(products);
        }
    }
}
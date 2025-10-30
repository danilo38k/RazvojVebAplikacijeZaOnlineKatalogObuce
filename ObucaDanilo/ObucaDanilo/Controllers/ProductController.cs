using Microsoft.AspNetCore.Mvc;
using ObucaDanilo.Models.ViewModels;
using ObucaDanilo.Repositories;

namespace ShoeCatalog.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        // konstruktor - injektuje repozitorijume proizvoda i kategorija
        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // prikazuje detalje proizvoda
        public async Task<IActionResult> Details(string id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var viewModel = new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryNames = product.ProductCategories.Select(pc => pc.Category.Name).ToList()
            };

            return View(viewModel);
        }

        // prikazuje proizvode iz određene kategorije
        public async Task<IActionResult> Category(string id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            ViewBag.CategoryName = category.Name;
            ViewBag.CategoryId = id;
            return View();
        }

        // učitava proizvode po strani (za lazy loading)
        [HttpGet]
        public async Task<IActionResult> LoadProducts(string categoryId, int skip = 0, int take = 8)
        {
            var products = await _productRepository.GetByCategoryPagedAsync(categoryId, skip, take);

            var productViewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryNames = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
            });

            return PartialView("_ProductCardPartial", productViewModels);
        }

        // pretraga proizvoda po terminu
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return RedirectToAction("Index", "Home");

            var products = await _productRepository.SearchAsync(searchTerm);

            var productViewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryNames = p.ProductCategories.Select(pc => pc.Category.Name).ToList()
            }).ToList();

            ViewBag.SearchTerm = searchTerm;
            return View("SearchResults", productViewModels);
        }
    }
}
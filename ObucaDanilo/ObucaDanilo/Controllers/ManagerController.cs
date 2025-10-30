using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ObucaDanilo.Models;
using ObucaDanilo.Repositories;

namespace ShoeCatalog.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _env;

        public ManagerController(
            ICategoryRepository categoryRepo,
            IProductRepository productRepo,
            IWebHostEnvironment env)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _env = env;
        }

        public IActionResult Index() => View();

        // Proizvodi - full CRUD

        public async Task<IActionResult> Products()
        {
            var products = await _productRepo.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(
            Product model,
            IFormFile imageFile,
            List<string> selectedCategories)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepo.GetAllAsync();
                return View(model);
            }

            if (imageFile?.Length > 0)
            {
                var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(imagesFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var filePath = Path.Combine(imagesFolder, fileName);
                using var fs = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fs);

                model.ImageUrl = "/images/" + fileName;
            }

            model.ProductCategories = selectedCategories?
                .Select(cid => new ProductCategory { CategoryId = cid })
                .ToList() ?? new List<ProductCategory>();

            await _productRepo.AddAsync(model);
            TempData["SuccessMessage"] = $"Proizvod '{model.Title}' je uspešno dodat.";
            return RedirectToAction(nameof(Products));
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(string id)
        {
            var model = await _productRepo.GetByIdAsync(id);
            if (model == null) return NotFound();

            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            ViewBag.SelectedCategories = model.ProductCategories
                .Select(pc => pc.CategoryId)
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(
            Product model,
            IFormFile? imageFile,
            List<string> selectedCategories)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepo.GetAllAsync();
                ViewBag.SelectedCategories = selectedCategories;
                return View(model);
            }

            var existingProduct = await _productRepo.GetByIdAsync(model.Id);
            if (existingProduct == null)
                return NotFound();

            existingProduct.Title = model.Title;
            existingProduct.Description = model.Description;
            existingProduct.Price = model.Price;
            existingProduct.UpdatedAt = DateTime.Now;

            if (imageFile != null && imageFile.Length > 0)
            {
                var imagesFolder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(imagesFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var filePath = Path.Combine(imagesFolder, fileName);
                using var fs = new FileStream(filePath, FileMode.Create);
                await imageFile.CopyToAsync(fs);

                existingProduct.ImageUrl = "/images/" + fileName;
            }

            existingProduct.ProductCategories = selectedCategories?
                .Select(cid => new ProductCategory
                {
                    ProductId = existingProduct.Id,
                    CategoryId = cid
                })
                .ToList() ?? new List<ProductCategory>();

            await _productRepo.UpdateAsync(existingProduct);
            TempData["SuccessMessage"] = $"Proizvod '{existingProduct.Title}' je uspešno izmenjen.";
            return RedirectToAction(nameof(Products));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var model = await _productRepo.GetByIdAsync(id);
            if (model != null)
            {
                await _productRepo.DeleteAsync(model);
                TempData["SuccessMessage"] = $"Proizvod '{model.Title}' je uspešno obrisan.";
            }
            return RedirectToAction(nameof(Products));
        }
    }
}
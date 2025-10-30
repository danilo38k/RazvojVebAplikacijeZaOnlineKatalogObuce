using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObucaDanilo.Models;
using ObucaDanilo.Repositories;
using System.Linq;
using System;

// Alias da izbegnemo konflikt sa Controller.User (ClaimsPrincipal)
using UserEntity = ObucaDanilo.Models.User;

namespace ShoeCatalog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;

        public AdminController(
            ICategoryRepository categoryRepo,
            IProductRepository productRepo,
            IUserRepository userRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        // pocetna admin
        public IActionResult Index() => View();

        #region kategorije

        public async Task<IActionResult> Categories()
            => View(await _categoryRepo.GetAllAsync());

        [HttpGet]
        public IActionResult AddCategory() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category c)
        {
            // DisplayOrder ne dolazi iz forme – izbaci ga iz validacije
            ModelState.Remove(nameof(Category.DisplayOrder));

            if (!ModelState.IsValid)
                return View(c);

            // Sledi sledeći raspoloživi redosled (1,2,3,...)
            var all = await _categoryRepo.GetAllAsync();
            c.DisplayOrder = all.Any() ? all.Max(x => x.DisplayOrder) + 1 : 1;

            await _categoryRepo.AddAsync(c);
            TempData["SuccessMessage"] = $"Kategorija '{c.Name}' je uspešno dodata.";
            return RedirectToAction(nameof(Categories));
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(string id)
        {
            var c = await _categoryRepo.GetByIdAsync(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category c)
        {
            // Ne vezujemo DisplayOrder iz forme i ne menjamo ga ovde
            ModelState.Remove(nameof(Category.DisplayOrder));

            if (!ModelState.IsValid)
                return View(c);

            var existing = await _categoryRepo.GetByIdAsync(c.Id);
            if (existing == null) return NotFound();

            // Ažuriramo samo ono što korisnik menja
            existing.Name = c.Name;
            existing.Description = c.Description;
            existing.UpdatedAt = DateTime.Now;

            await _categoryRepo.UpdateAsync(existing);
            TempData["SuccessMessage"] = $"Kategorija '{existing.Name}' je uspešno izmenjena.";
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var c = await _categoryRepo.GetByIdAsync(id);
            if (c != null)
            {
                await _categoryRepo.DeleteAsync(c);
                TempData["SuccessMessage"] = $"Kategorija '{c.Name}' je uspešno obrisana.";
            }
            return RedirectToAction(nameof(Categories));
        }

        #endregion

        #region proizvodi (samo pregled - bez upravljanja)

        public async Task<IActionResult> Products()
        {
            var products = await _productRepo.GetAllAsync();
            return View(products);
        }

        #endregion

        #region korisnici

        public async Task<IActionResult> Users()
        {
            var users = await _userRepo.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserEntity model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Id = Guid.NewGuid().ToString();
            model.CreatedAt = DateTime.Now;
            await _userRepo.AddAsync(model);

            TempData["SuccessMessage"] = $"Korisnik '{model.Email}' je uspešno dodat.";
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserEntity model)
        {
            // Password tokom izmene tretiramo kao OPCIONI:
            // eksplicitno referenciramo svoj User model, ne ClaimsPrincipal.User
            ModelState.Remove(nameof(UserEntity.Password));

            if (!ModelState.IsValid)
                return View(model);

            var existing = await _userRepo.GetByIdAsync(model.Id);
            if (existing == null) return NotFound();

            existing.FirstName = model.FirstName;
            existing.LastName = model.LastName;
            existing.Email = model.Email;

            // Menjaj lozinku samo ako je unesena
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                existing.Password = model.Password;
            }

            existing.IsAdmin = model.IsAdmin;

            // Ako imaš IsManager u modelu (po tvojoj šemi, imaš)
            existing.IsManager = model.IsManager;

            existing.UpdatedAt = DateTime.Now;

            await _userRepo.UpdateAsync(existing);

            TempData["SuccessMessage"] = $"Korisnik '{existing.Email}' je uspešno izmenjen.";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var model = await _userRepo.GetByIdAsync(id);
            if (model != null)
            {
                await _userRepo.DeleteAsync(model);
                TempData["SuccessMessage"] = $"Korisnik '{model.Email}' je uspešno obrisan.";
            }
            return RedirectToAction(nameof(Users));
        }

        #endregion
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ObucaDanilo.Models;
using ObucaDanilo.Repositories;
using System.Security.Claims;

namespace ShoeCatalog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
            => _userRepository = userRepository;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            // Use Password (renamed from PasswordHash)
            if (user == null || user.Password != password)
            {
                ModelState.AddModelError("", "Pogrešan email ili lozinka.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            if (user.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            if (user.IsManager)
                claims.Add(new Claim(ClaimTypes.Role, "Manager"));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            // Prefer Admin panel if Admin, then Manager panel; otherwise go ReturnUrl/Home
            if (user.IsAdmin)
                return RedirectToAction("Index", "Admin");

            if (user.IsManager)
                return RedirectToAction("Index", "Manager");

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (await _userRepository.GetByEmailAsync(user.Email) != null)
            {
                ModelState.AddModelError("Email", "Email adresa je već registrovana.");
                return View(user);
            }

            user.Id = Guid.NewGuid().ToString();
            user.CreatedAt = DateTime.Now;
            user.IsAdmin = false;
            user.IsManager = false;

            await _userRepository.AddAsync(user);

            TempData["SuccessMessage"] = "Uspešna registracija! Prijavite se.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
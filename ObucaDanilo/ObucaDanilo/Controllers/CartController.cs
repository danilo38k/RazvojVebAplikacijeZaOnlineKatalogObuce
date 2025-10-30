using Microsoft.AspNetCore.Mvc;
using ObucaDanilo.Models.ViewModels;
using ObucaDanilo.Services;

namespace ObucaDanilo.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        // konstruktor - injektuje servis za korpu
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // prikazuje korpu korisnika
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _cartService.GetCartItemsAsync(userId);
            var viewModel = cartItems.Select(c => new CartViewModel
            {
                Id = c.Id,
                ProductId = c.ProductId,
                ProductTitle = c.Product.Title,
                ProductImage = c.Product.ImageUrl,
                ProductPrice = c.Product.Price,
                Quantity = c.Quantity
            }).ToList();

            return View(viewModel);
        }

        // dodaje proizvod u korpu
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productId, int quantity = 1)
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            await _cartService.AddToCartAsync(userId, productId, quantity);
            return RedirectToAction("Index");
        }

        // ažurira količinu za stavku u korpi
        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(string cartItemId, int quantity)
        {
            await _cartService.UpdateCartItemAsync(cartItemId, quantity);
            return RedirectToAction("Index");
        }

        // uklanja stavku iz korpe
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return RedirectToAction("Index");
        }

        // prazni korpu korisnika
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            await _cartService.ClearCartAsync(userId);
            return RedirectToAction("Index");
        }
    }
}
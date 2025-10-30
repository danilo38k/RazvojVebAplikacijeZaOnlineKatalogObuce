using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObucaDanilo.Models;
using ObucaDanilo.Models.ViewModels;
using ObucaDanilo.Repositories;
using ObucaDanilo.Services;

namespace ShoeCatalog.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;

        // konstruktor - injektuje repozitorijum porudžbina i servis za korpu
        public OrderController(
            IOrderRepository orderRepository,
            ICartService cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
        }

        // prikazuje sve porudžbine korisnika
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderRepository.GetByUserIdAsync(userId);
            var viewModel = orders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                ShippingAddress = o.ShippingAddress,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Items = o.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    ProductTitle = oi.Product.Title,
                    ProductImage = oi.Product.ImageUrl,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();

            return View(viewModel);
        }

        // prikazuje checkout stranicu
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _cartService.GetCartItemsAsync(userId);
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var total = await _cartService.GetCartTotalAsync(userId);
            ViewBag.CartTotal = total;

            return View();
        }

        // obrađuje checkout i kreira porudžbinu
        [HttpPost]
        public async Task<IActionResult> Checkout(string shippingAddress)
        {
            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _cartService.GetCartItemsAsync(userId);
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                UserId = userId,
                ShippingAddress = shippingAddress,
                TotalAmount = await _cartService.GetCartTotalAsync(userId),
                Status = "Processing",
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    UnitPrice = c.Product.Price
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            await _cartService.ClearCartAsync(userId);

            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }

        // prikazuje potvrdu porudžbine
        public async Task<IActionResult> OrderConfirmation(string id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.UserId != User.Identity?.Name)
            {
                return NotFound();
            }

            var viewModel = new OrderViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductId = oi.ProductId,
                    ProductTitle = oi.Product.Title,
                    ProductImage = oi.Product.ImageUrl,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
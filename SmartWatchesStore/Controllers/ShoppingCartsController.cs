using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWatchesStore.Data;
using SmartWatchesStore.Models;
using SmartWatchesStore.ViewModels;
using System.Diagnostics;

namespace SmartWatchesStore.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly AppDbContext _context;

        public ShoppingCartsController(AppDbContext context)
        {
             _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var cartProducts = await _context.ShoppingCarts.ToListAsync();
            return View(cartProducts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", product);
            }

            var shoppingCart = new ShoppingCart
            {
                Name = product.Name,
                Color = product.Color,
                Price = product.Price,
                Quantity = 1
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

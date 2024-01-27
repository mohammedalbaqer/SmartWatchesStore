using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SmartWatchesStore.Data;
using SmartWatchesStore.Models;
using SmartWatchesStore.ViewModels;

namespace SmartWatchesStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> IndexAdmin()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> IndexProducts()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {            
            return View("ProductForm", new ProductFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {                
                return View("ProductForm", model);
            }

            var files = Request.Form.Files;

            if (!files.Any())
            {                
                ModelState.AddModelError("Poster", "Please select movie poster!");
                return View("ProductForm", model);
            }

            var poster = files.FirstOrDefault();

            if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {                
                ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                return View("ProductForm", model);
            }

            if (poster.Length > _maxAllowedPosterSize)
            {                
                ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                return View("ProductForm", model);
            }

            using var dataStream = new MemoryStream();

            await poster.CopyToAsync(dataStream);

            var products = new Product
            {
                Name = model.Name,
                Color = model.Color,
                Price = model.Price,
                Description = model.Description,
                Poster = dataStream.ToArray()
            };

            _context.Products.Add(products);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Product created successfully!");
            return RedirectToAction(nameof(IndexAdmin));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            var viewModel = new ProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Color = product.Color,
                Price = product.Price,
                Description = product.Description,
                Poster = product.Poster
            };

            return View("ProductForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ProductForm", model);
            }

            var product = await _context.Products.FindAsync(model.Id);

            if (product == null)
                return NotFound();

            var files = Request.Form.Files;

            if (files.Any())
            {
                var poster = files.FirstOrDefault();

                using var dataStream = new MemoryStream();

                await poster.CopyToAsync(dataStream);

                model.Poster = dataStream.ToArray();

                if (!_allowedExtenstions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {                    
                    ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed!");
                    return View("ProductForm", model);
                }

                if (poster.Length > _maxAllowedPosterSize)
                {                    
                    ModelState.AddModelError("Poster", "Poster cannot be more than 1 MB!");
                    return View("ProductForm", model);
                }

                product.Poster = model.Poster;
            }

            product.Name = model.Name;
            product.Color = model.Color;
            product.Price = model.Price;
            product.Description = model.Description;

            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Product updated successfully!");
            return RedirectToAction(nameof(IndexAdmin));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("IndexAdmin");
        }


    }
}

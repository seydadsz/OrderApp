using Microsoft.AspNetCore.Mvc;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepo;

        public ProductsController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        // Ürün listesi
        public async Task<IActionResult> Index()
        {
            var items = await _productRepo.GetAllAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool exists = await _productRepo.StockCodeExistsAsync(model.StockCode);
            if (exists)
            {
                ModelState.AddModelError("", "\r\nThis stock code already exists.");
                return View(model);
            }

            await _productRepo.AddAsync(model);
            TempData["Success"] = "\r\nThe product has been added successfully.";
            return RedirectToAction("Index");
        }

        // Silme onay sayfası
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // Silme işlemi
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            // sipariş kontrolü
            bool hasOrders = await _productRepo.HasOrdersAsync(id);
            if (hasOrders)
            {
                TempData["Error"] = "\r\nThis product cannot be deleted because it is used in orders.";
                return RedirectToAction("Index");
            }

            // ürün + fiyat geçmişi silinir
            await _productRepo.DeleteAsync(id);

            TempData["Success"] = "The product was deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}

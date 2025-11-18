using Microsoft.AspNetCore.Mvc;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Controllers
{
    public class ProductPricesController : Controller
    {
        private readonly IProductPriceRepository _priceRepo;
        private readonly IProductRepository _productRepo;

        public ProductPricesController(IProductPriceRepository priceRepo, IProductRepository productRepo)
        {
            _priceRepo = priceRepo;
            _productRepo = productRepo;
        }

        // Fiyat listesini göster
        public async Task<IActionResult> Index()
        {
            var list = await _priceRepo.GetAllWithProductsAsync();
            return View(list);
        }

        // Yeni fiyat ekleme
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productRepo.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductPrice model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _productRepo.GetAllAsync();
                return View(model);
            }

            model.ValidFrom = DateTime.Now;

            await _priceRepo.AddAsync(model);

            TempData["Success"] = "\r\nNew price added successfully.";
            return RedirectToAction("Index");
        }

        // Son fiyatı düzenleme
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _priceRepo.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            var product = await _productRepo.GetByIdAsync(item.ProductId);
            ViewBag.ProductName = product!.StockName;

            return View(item);
        }


        [HttpPost]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductPrice model)
        {
            if (!ModelState.IsValid)
            {
                var product = await _productRepo.GetByIdAsync(model.ProductId);
                ViewBag.ProductName = product!.StockName;
                return View(model);
            }

            // Yeni fiyat kaydı ekle
            model.Id = 0;
            model.ValidFrom = DateTime.Now;

            await _priceRepo.AddAsync(model);

            TempData["Success"] = "\r\nNew price added successfully.";
            return RedirectToAction("Index");
        }



        // Yeni fiyat girme (geçmişe ekleme)
        public async Task<IActionResult> UpdatePrice(int productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return NotFound();

            var latestPrice = await _priceRepo.GetLatestPriceAsync(productId);
            ViewBag.LatestPrice = latestPrice;

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice(int productId, decimal price)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                return NotFound();

            var model = new ProductPrice
            {
                ProductId = productId,
                Price = price,
                ValidFrom = DateTime.Now
            };

            await _priceRepo.AddAsync(model);

            TempData["Success"] = "\r\nPrice updated successfully.";
            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> History(int id)
        {
            // Yeni repository metodunu kullanıyoruz
            var prices = await _priceRepo.GetPricesByProductIdAsync(id);

            if (prices == null || prices.Count == 0)
                return NotFound("\r\nNo price history found for this product.");

            ViewBag.ProductName = prices.First().StockName;

            return View(prices);
        }
    }
}

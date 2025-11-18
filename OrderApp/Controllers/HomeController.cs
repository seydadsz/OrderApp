using Microsoft.AspNetCore.Mvc;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IProductPriceRepository _priceRepo;
        private readonly IOrderRepository _orderRepo;

        public HomeController(
            IProductRepository productRepo,
            ICustomerRepository customerRepo,
            IProductPriceRepository priceRepo,
            IOrderRepository orderRepo)
        {
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _priceRepo = priceRepo;
            _orderRepo = orderRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            var customers = await _customerRepo.GetAllAsync();
            var orders = await _orderRepo.GetAllWithDetailsAsync();
            var prices = await _priceRepo.GetAllWithProductsAsync();

            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalCustomers = customers.Count;
            ViewBag.TotalOrders = orders.Count;
            ViewBag.PendingOrders = orders.Count(o => !o.IsConfirmed);
            ViewBag.TotalPrices = prices.Count;

            ViewBag.LastOrder = orders.FirstOrDefault();

            return View();
        }
    }
}

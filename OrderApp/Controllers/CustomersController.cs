using Microsoft.AspNetCore.Mvc;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;

namespace OrderApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _repo;

        public CustomersController(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _repo.GetAllAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool exists = await _repo.CustomerCodeExistsAsync(model.CustomerCode);
            if (exists)
            {
                ModelState.AddModelError("", "\r\nThis customer code already exists.");
                return View(model);
            }

            await _repo.AddAsync(model);

            TempData["Success"] = "\r\nThe customer has been added successfully.";
            return RedirectToAction("Index");
        }

        // Silme onay 
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _repo.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // Güvenli silme — Siparişi varsa ENGELLE
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Önce sipariş var mı kontrol et
            bool hasOrders = await _repo.HasOrdersAsync(id);

            if (hasOrders)
            {
                TempData["Error"] = "\r\nThis customer cannot be deleted because it is used in orders.";
                return RedirectToAction("Index");
            }

            // sil
            await _repo.DeleteAsync(id);

            TempData["Success"] = "\r\nThe client was deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}

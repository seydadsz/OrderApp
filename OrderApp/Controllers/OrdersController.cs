using Microsoft.AspNetCore.Mvc;
using OrderApp.Models;
using OrderApp.Repositories.Interfaces;
using OrderApp.Services;
using System.Net;

namespace OrderApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IProductPriceRepository _priceRepo;
        private readonly EmailService _emailService;

        public OrdersController(
            IOrderRepository orderRepo,
            IProductRepository productRepo,
            ICustomerRepository customerRepo,
            IProductPriceRepository priceRepo,
            EmailService emailService)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _priceRepo = priceRepo;
            _emailService = emailService;
        }

        
        // INDEX
        
        public async Task<IActionResult> Index()
        {
            var items = await _orderRepo.GetAllWithDetailsAsync();
            return View(items);
        }

        
        // CREATE GET
        
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productRepo.GetAllAsync();
            ViewBag.Customers = await _customerRepo.GetAllAsync();
            return View();
        }

        
        // CREATE POST
       
        [HttpPost]
        public async Task<IActionResult> Create(Order model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _productRepo.GetAllAsync();
                ViewBag.Customers = await _customerRepo.GetAllAsync();
                return View(model);
            }

            // Sipariş numarası
            model.OrderNumber = "ORD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            // Son fiyatı çek
            var price = await _priceRepo.GetLatestPriceAsync(model.ProductId);
            if (price == null)
            {
                ModelState.AddModelError("", "No valid price found for this product.");
                return View(model);
            }

            model.UnitPrice = price.Value;

            // Token oluştur
            model.ConfirmationToken = Guid.NewGuid().ToString();
            model.CreatedAt = DateTime.Now;

            // Kaydet
            await _orderRepo.AddAsync(model);

            
            // EMAIL GÖNDERME
            
            var customer = await _customerRepo.GetByIdAsync(model.CustomerId);

            if (customer != null && !string.IsNullOrEmpty(customer.Email))
            {
                // Customer güvenli hale getir
                var safeCustomer = WebUtility.HtmlEncode(customer.CustomerName ?? "");

                // URL oluşturma
                string confirmUrl = Url.Action(
                    "Confirm",
                    "Orders",
                    new { token = model.ConfirmationToken },
                    Request.Scheme
                ) ?? throw new Exception("Confirm URL could not be generated.");


                // HTML e-posta template
                string body = $@"
<html>
<body style='font-family: Arial; background-color:#f6f6f6; padding:20px;'>

<div style='max-width:600px; margin:auto; background:#fff; padding:25px; border-radius:8px; 
            border:1px solid #ddd;'>

    <h2 style='color:#0d6efd;'>OrderApp – Order Confirmation</h2>

    <p>Hello <strong>{safeCustomer}</strong>,</p>

    <p>Your order has been created successfully.</p>

    <p>
        <strong>Order Number:</strong> {model.OrderNumber}<br/>
        <strong>Quantity:</strong> {model.Quantity}<br/>
        <strong>Unit Price:</strong> {model.UnitPrice} ₺<br/>
    </p>

    <p>Please click the button below to confirm your order:</p>

    <p style='margin:30px 0;'>
        <a href=""{confirmUrl}""
           style='padding:12px 20px; background:#0d6efd; color:white; 
                  text-decoration:none; border-radius:5px; font-weight:bold;'>
            Confirm Order
        </a>
    </p>

    <p>If the button does not work, copy and paste the link below:</p>
    <p style='color:#0d6efd; word-break:break-all;'>{confirmUrl}</p>

    <hr style='margin:30px 0; border:none; border-top:1px solid #eee;' />

    <p style='font-size:12px; color:#888;'>This is an automated message from OrderApp.</p>

</div>

</body>
</html>";

                await _emailService.SendEmailAsync(
                    customer.Email,
                    "Order Confirmation",
                    body
                );
            }

            return RedirectToAction("Index");
        }

        // ------------------------------------------------------------
        // CONFIRM ORDER (TOKEN İLE)
        // ------------------------------------------------------------
        public async Task<IActionResult> Confirm(string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest();

            var order = await _orderRepo.GetByTokenAsync(token);
            if (order == null)
                return NotFound();

            await _orderRepo.ConfirmAsync(order.Id);

            return View(order);
        }
    }
}

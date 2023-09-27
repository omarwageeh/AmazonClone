using AmazonClone.Model;
using AmazonClone.Service;
using AmazonClone.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AmazonClone.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ProductService _productService;
        private readonly OrderSerivce _orderService;
        public CartController(ProductService productService, OrderSerivce orderSerivce)
        {
            _productService = productService;
            _orderService = orderSerivce;
        }
        private async Task<Cart> GetCartFromSession()
        {
            await HttpContext.Session.LoadAsync();
            var sessionString = HttpContext.Session.GetString("cart");
            if (sessionString is not null)
            {
                return JsonSerializer.Deserialize<Cart>(sessionString)!;
            }

            return new Cart();
        }
        public async Task<IActionResult> Index()
        {
            // Get the value of the session
            var data = await GetCartFromSession();

            //Pass the list to the view to render
            return View(data);
        }
        public async Task<IActionResult> Checkout()
        {
            var cart = await GetCartFromSession();
            var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier);
            Guid id = Guid.Parse(claim?.Value!);
            var order  = await _orderService.AddOrder(new Order() { CustomerId = id, Status= Model.Enum.Status.Pending, TotalPrice = cart.TotalPrice });
            foreach (var cartItem in cart.Items)
            {
                await _orderService.AddOrderDetails(new OrderDetails() { OrderId = order.Id, ProductId = cartItem.Product.Id, ProductCount = cartItem.ProductCount, UnitPrice = cartItem.Product.UnitPrice });
            }
            
            return RedirectToAction(nameof(Index), "Home");
        }

    }
}

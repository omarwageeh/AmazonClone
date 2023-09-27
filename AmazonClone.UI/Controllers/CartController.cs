using AmazonClone.Model;
using AmazonClone.Service;
using AmazonClone.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazonClone.UI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly OrderSerivce _orderService;
        private readonly CartService _cartService;
        public CartController(OrderSerivce orderSerivce, CartService cartService)
        {
            _orderService = orderSerivce;
            _cartService = cartService;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _cartService.GetCartFromSession();

            return View(data);
        }
        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartService.GetCartFromSession();

            var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.NameIdentifier);
            Guid customerId = Guid.Parse(claim?.Value!);

            var order = await _orderService.AddOrder(new Order() { CustomerId = customerId, Status = Model.Enum.Status.Pending, TotalPrice = cart.TotalPrice });

            foreach (var cartItem in cart.Items)
            {
                await _orderService.AddOrderDetails(new OrderDetails() { OrderId = order.Id, ProductId = cartItem.Product.Id, ProductCount = cartItem.ProductCount, UnitPrice = cartItem.Product.UnitPrice });
            }
            
            return RedirectToAction(nameof(Index), "Home");
        }

    }
}

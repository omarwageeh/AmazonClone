using AmazonClone.Model;
using AmazonClone.Service;
using AmazonClone.UI.Models;
using AmazonClone.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AmazonClone.UI.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrderSerivce _orderService;
        public OrdersController(OrderSerivce orderSerivce)
        {
            _orderService = orderSerivce;
        }

        public async Task<IActionResult> Index()
        {
            var customerId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)!.Value;
            var orders = await _orderService.GetAllOrderByCustomerId(customerId);
            OrderViewModel orderViewModel = new OrderViewModel();
            foreach (var item in orders)
            {
                var orderDetail = await _orderService.GetAllOrderDetails(item.Id, true);
                orderViewModel.Orders.Add(new OrderModel() { Order = item, OrderDetails = orderDetail.ToList()});
            }
            return View(orderViewModel);
        }


    }
}

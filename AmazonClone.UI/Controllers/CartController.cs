using AmazonClone.Model;
using AmazonClone.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace AmazonClone.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductService _productService;
        public CartController(ProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            // Get the value of the session
            var data = await GetBooksFromSession();

            //Pass the list to the view to render
            return View(data);
        }

        private async Task<List<Product>> GetBooksFromSession()
        {
            await HttpContext.Session.LoadAsync();
            var sessionString = HttpContext.Session.GetString("cart");
            if (sessionString is not null)
            {
                return JsonSerializer.Deserialize<List<Product>>(sessionString)!;
            }

            return Enumerable.Empty<Product>().ToList();
        }
        public async Task<IActionResult> AddToCart(Guid id)
        {
            var data = await GetBooksFromSession();

            var product =  (await _productService.GetProducts(p => p.Id == id))!.FirstOrDefault();

            if (product is not null)
            {
                data.Add(product);

                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(data));

                TempData["Success"] = "The product is added successfully";

                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}

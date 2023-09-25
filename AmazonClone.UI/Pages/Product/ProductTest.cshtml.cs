using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AmazonClone.Model;
using AmazonClone.Service;
using System.Text;
using System.Text.Json;
using AmazonClone.UI.Models;

namespace AmazonClone.UI.Pages.Products
{
    public class ProductTestModel : PageModel
    {
        private readonly ProductService _productService;
        public ProductTestModel(ProductService productService)
        {
            _productService = productService;
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

        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var products = await _productService.GetProducts(p => !p.IsDeleted);
            if(products!=null)
            {
               Product = products.ToList();
            }
           
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            Cart cart = await GetCartFromSession();

            var product = (await _productService.GetProducts(p => p.Id == id))!.FirstOrDefault();

            if (product is not null)
            {

                if(cart.Items.Exists(item => item.Product.Id == id))
                {
                    cart.Items.Find(item => item.Product.Id == id)!.ProductCount++;
                }
                else
                {
                    ProductModel productModel = new ProductModel(product);
                    cart.Items.Add(new CartItem(productModel, 1));
                }                
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(cart));

                TempData["Success"] = "The product is added successfully";

                return RedirectToPage("/Product/ProductTest");
            }

            return NotFound();
        }



    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AmazonClone.Model;
using AmazonClone.Service;

namespace AmazonClone.UI.Pages.Products
{
    public class ProductCreateModel : PageModel
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductCreateModel(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            
          if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await _productService.AddProduct(Product);
            return RedirectToPage("./Index");
        }
    }
}

using AmazonClone.Service;
using AmazonClone.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AmazonClone.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger , ProductService productService, CategoryService categoryService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var dataContext = await _categoryService.GetCategories(p => !p.IsDeleted);
            return View(dataContext);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
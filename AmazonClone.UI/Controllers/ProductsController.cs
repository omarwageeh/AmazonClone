using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmazonClone.Data.Context;
using AmazonClone.Model;
using AmazonClone.Repository.UnitOfWork;
using AmazonClone.Repository.Interface;
using AmazonClone.Service;
using Microsoft.CodeAnalysis;

namespace AmazonClone.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductsController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: Products
        public async Task<IActionResult> Index(Guid? CategoryId)
        {
            var dataContext = await _productService.GetProducts(p => p.CategoryId == CategoryId);
            return View(dataContext);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            var product =  await _productService.GetProducts(p => p.Id == productId);

            if (product?.FirstOrDefault() == null)
            {
                return NotFound();
            }

            return View(product.FirstOrDefault());


            //// Retrieve the product details from your data source
            //var product = _productService.GetProducts(p => p.Id == productId);

            //// Pass the product details to the view
            //return View(product);
        }

        // GET: Products/Create
        public async Task< IActionResult> Create()
        {
            var categories =await _categoryService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameEn,NameAr,UnitPrice,StockQuantity,CategoryId,Id,CreatedOn,IsDeleted")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }
    }
}

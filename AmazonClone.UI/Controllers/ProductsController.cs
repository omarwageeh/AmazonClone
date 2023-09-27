using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AmazonClone.Data.Context;
using AmazonClone.Model;

using AmazonClone.Service;
using Microsoft.CodeAnalysis;
using AmazonClone.UI.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using AmazonClone.UI.Services;

namespace AmazonClone.UI.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly CartService _cartService;

        public ProductsController(ProductService productService, CategoryService categoryService, CartService cartService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
        }


        // GET: Products
        public async Task<IActionResult> Index(Guid? categoryId)
        { 
            if (categoryId is not null)
            {
                var dataContext = await _productService.GetProducts(p => p.CategoryId == categoryId);
                return View(dataContext);
            }
            return View(await _productService.GetProducts(p => !p.IsDeleted));
            
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
            ProductDetailsModel productDetailsModel = new ProductDetailsModel(new ProductModel(product.FirstOrDefault()!));
            return View(productDetailsModel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(Guid id, int quantity = 1)
        {
            Cart cart = await _cartService.GetCartFromSession();

            var product = (await _productService.GetProducts(p => p.Id == id))!.FirstOrDefault();

            if (product is not null)
            {

                if (cart.Items.Exists(item => item.Product.Id == id))
                {
                    cart.Items.Find(item => item.Product.Id == id)!.ProductCount += quantity;
                }
                else
                {
                    ProductModel productModel = new ProductModel(product);
                    cart.Items.Add(new CartItem(productModel, quantity));
                }
                 _cartService.SetCart(cart);
                

                TempData["Success"] = "The product is added successfully";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

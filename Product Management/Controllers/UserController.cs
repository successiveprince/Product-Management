using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Models.Domain;
using System.Reflection.Metadata.Ecma335;
using Product_Management.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Product_Management.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductDbContext _context;

        public UserController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> UserHomePage()
        {
            var allProducts = _context.Products.ToList();
            var productList = await _context.Products.ToListAsync();
            var categoryList = await _context.Categories.ToListAsync();
            var categoryProduct = productList.Join(// outer sequence 
                       categoryList,  // inner sequence 
                       product => product.ProductCategoryId,   // outerKeySelector
                       category => category.CategoryId, // innerKeySelector
                       (product, category) => new CategoryProductDto // result selector
                       {
                           ProductId = product.ProductId,
                           ProductName = product.ProductName,
                           ProductDescription = product.ProductDescription,
                           ProductPrice = product.ProductPrice,
                           ProductImage = product.ProductImage,
                           IsAvailable = product.IsAvailable,
                           IsActive = product.IsActive,
                           ProductCreatedAt = product.ProductCreatedAt,
                           IsTrending = product.IsTrending,
                           CategoryId = category.CategoryId,
                           CategoryName = category.CategoryName
                       }).OrderByDescending(x => x.ProductCreatedAt).ToList();
            ViewBag.CategoryList = await _context.Categories.ToListAsync();
           
            return View(categoryProduct);
           
        }
        
    }

}

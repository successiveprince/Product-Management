using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.Dto;

namespace Product_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProductDbContext _context;
        public AdminController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AdminHome()
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
            return View(categoryProduct);
        }


    }
}

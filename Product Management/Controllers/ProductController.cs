using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.Domain;
using Product_Management.Models.Dto;

namespace Product_Management.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductDbContext _productDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ProductDbContext productDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _productDbContext = productDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var productList = await _productDbContext.Products.ToListAsync();
            var categoryList = await _productDbContext.Categories.ToListAsync();
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
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.CategoryList = await _productDbContext.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDto addProductDto)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = "";
                if (addProductDto.ProductImage != null)
                {
                    string uploadFoler = Path.Combine(_webHostEnvironment.WebRootPath, "image");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + addProductDto.ProductImage.FileName;
                    string filePath = Path.Combine(uploadFoler, uniqueFileName);
                    addProductDto.ProductImage.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                var product = new Product
                {
                    ProductName = addProductDto.ProductName,
                    ProductDescription = addProductDto.ProductDescription,
                    ProductPrice = addProductDto.ProductPrice,
                    IsAvailable = addProductDto.IsAvailable,
                    //IsActive = addProductDto.IsActive,
                    IsTrending = addProductDto.IsTrending,
                    ProductCreatedAt = DateTime.Now,
                    ProductCategoryId = addProductDto.ProductCategoryId,
                    ProductImage = uniqueFileName
                };
                _productDbContext.Add(product);
                await _productDbContext.SaveChangesAsync();
                ViewBag.Success = "Product added successfully";
                
                return RedirectToAction("AdminHome" , "Admin");
            }
            return View(addProductDto);
        }
    }
}

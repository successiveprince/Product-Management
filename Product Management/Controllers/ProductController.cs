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

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            ViewBag.CategoryList = await _productDbContext.Categories.ToListAsync();
            if (product != null)
            {
                var newProduct = new UpdateProductDto()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductDescription = product.ProductDescription,
                    IsAvailable = product.IsAvailable,
                    IsTrending = product.IsTrending,
                    ProductCategoryId = product.ProductCategoryId,
                    Category = product.Category,
                };
                return await Task.Run(() => View("UpdateProduct", newProduct));
            }
            return RedirectToAction("AdminHome" , "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
        {
            var product = await _productDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == updateProductDto.ProductId);
            ViewBag.CaterogyList = _productDbContext.Categories.ToList();

            string uniqueFileName = "";
            if (updateProductDto.ProductImage != null)
            {
                string uploadFoler = Path.Combine(_webHostEnvironment.WebRootPath, "image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + updateProductDto.ProductImage.FileName;
                string filePath = Path.Combine(uploadFoler, uniqueFileName);
                updateProductDto.ProductImage.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            if (product != null)
            {
                product.ProductName = updateProductDto.ProductName;
                product.ProductPrice = updateProductDto.ProductPrice;
                product.ProductDescription = updateProductDto.ProductDescription;
                product.ProductImage = uniqueFileName;
                product.IsAvailable = updateProductDto.IsAvailable;
                product.IsTrending = updateProductDto.IsTrending;
                product.ProductCategoryId = updateProductDto.ProductCategoryId;
            }
            await _productDbContext.SaveChangesAsync();
            return RedirectToAction("AdminHome", "Admin");
        }

        
    }
}

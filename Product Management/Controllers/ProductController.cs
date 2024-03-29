﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.Domain;
using Product_Management.Models.Dto;
using System;

namespace Product_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ProductDbContext _productDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ProductDbContext productDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _productDbContext = productDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        //Not using this GetAllProducts
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
            ViewBag.SelectedCategory = ViewBag.SelectedCategory ?? 0;
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
                    IsAvailable = true,
                   
                    IsTrending = addProductDto.IsTrending,
                    ProductCreatedAt = DateTime.Now,
                    ProductCategoryId = addProductDto.ProductCategoryId,
                    ProductImage = uniqueFileName
                };
                _productDbContext.Add(product);
                await _productDbContext.SaveChangesAsync();
                ViewBag.Success = "Product added successfully";
                TempData["productsuccess"] = "Product Added Successfully";

                return RedirectToAction("AdminHome" , "Admin");
            }
            else
            {
                // Display a message indicating that the form is not valid
                TempData["error"] = "Please fill in all required fields.";
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
                var newProduct = new Product()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductDescription = product.ProductDescription,
                    IsAvailable = product.IsAvailable,
                    IsTrending = product.IsTrending,
                    ProductCategoryId = product.ProductCategoryId,
                    Category = product.Category,
                    ProductImage = product.ProductImage
                 
                };
                return await Task.Run(() => View("UpdateProduct", newProduct));
            }
            return RedirectToAction("AdminHome" , "Admin");
        }


        private string UploadImage(UpdateProductDto model)
        {
            string uniqueFileName = string.Empty;
            if (model.ProductImage != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductImage.FileName;
                string FilePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    model.ProductImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductDto productModel)
        {
            var product = await _productDbContext.Products.FirstOrDefaultAsync(x => x.ProductId == productModel.ProductId);
            ViewBag.CaterogyList = _productDbContext.Categories.ToList();

            string uniqueFileName = "";
            if (productModel.ProductImage != null)
            {
                string uploadFoler = Path.Combine(_webHostEnvironment.WebRootPath, "image");
                if (!string.IsNullOrEmpty(product.ProductImage))
                {
                    string previousImagePath = Path.Combine(uploadFoler, product.ProductImage);
                    if (System.IO.File.Exists(previousImagePath))
                    {
                        System.IO.File.Delete(previousImagePath);
                    }
                }
                //uniqueFileName = Guid.NewGuid().ToString() + "_" + productModel.ProductImage.FileName;
                //string filePath = Path.Combine(uploadFoler, uniqueFileName);
                ////updateProductDto.MyProductImage.CopyTo(new FileStream(filePath, FileMode.Create));
                //productModel.ProductImage.CopyTo(new FileStream(filePath, FileMode.Create));
                uniqueFileName = UploadImage(productModel);
            }

            if (product != null)
            {
                product.ProductName = productModel.ProductName;
                product.ProductPrice = productModel.ProductPrice;
                product.ProductDescription = productModel.ProductDescription;
                if (productModel.ProductImage != null)
                {
                    product.ProductImage = uniqueFileName;
                }

                product.IsTrending = productModel.IsTrending;
                product.ProductCategoryId = productModel.ProductCategoryId;
            }
            await _productDbContext.SaveChangesAsync();
            TempData["productSuccess"] = "Product Updated Successfully";
            return RedirectToAction("AdminHome", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Active(int id)
        {
            var product = _productDbContext.Products.FirstOrDefault(x => x.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            product.IsAvailable = true;
            await _productDbContext.SaveChangesAsync();
            return RedirectToAction("Adminhome", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Deactive(int id)
        {
            var product = _productDbContext.Products.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            product.IsAvailable = false;
            await _productDbContext.SaveChangesAsync();
            return RedirectToAction("AdminHome", "Admin");
        }


    }


}

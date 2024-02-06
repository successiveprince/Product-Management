using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.Domain;
using Product_Management.Models.Dto;

namespace Product_Management.Controllers

{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;

        public AdminController(ProductDbContext context , SignInManager<UserModel> signInManager , UserManager<UserModel> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
       
        [HttpGet]
        public async Task<IActionResult> AdminHome()
        {
            //GetAll
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

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
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

            return Json(new { data = categoryProduct });
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            
            if (user != null)
            {
                var newUser = new UpdateUserDto()
                {
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNo = user.PhoneNo,
                    UserPassword = user.UserPassword,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = user.IsActive
                };
                return await Task.Run(() => View("UpdateUser", newUser));
            }
            return RedirectToAction("GetAllUser", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == updateUserDto.Id);

            if (user != null)
            {

                user.Name = updateUserDto.Name;
                user.PhoneNo = updateUserDto.PhoneNo;
                user.Email = updateUserDto.Email;
                user.CreatedAt = DateTime.UtcNow;
                user.IsActive = updateUserDto.IsActive;

                if (!string.IsNullOrEmpty(updateUserDto.UserPassword))
                {

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, updateUserDto.UserPassword);

                    if (!resetPasswordResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to reset the password.");
                        return View(updateUserDto);
                    }
                    user.UserPassword = updateUserDto.UserPassword;
                    user.ConfirmPassword = updateUserDto.UserPassword;
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("GetAllUser", "Auth");
            }
            return RedirectToAction("GetAllUser", "Auth");
        }

        [HttpGet]
        public IActionResult ViewProfile()
        {
            return View();
        }
    }
}

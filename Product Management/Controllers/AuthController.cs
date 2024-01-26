using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.Domain;
using Product_Management.Models.Dto;

namespace Product_Management.Controllers
{
    public class AuthController : Controller
    {
        private readonly ProductDbContext _productDbContext;
        
        public AuthController(ProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }
        [HttpGet]
        public IActionResult HomePage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Verify(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _productDbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == model.UserEmail);
                if (user != null)
                {
                    if (VerifyPassword(model.UserPassword , user.UserPassword))
                    {
                        // Password matched with the input email
                        //Redirect
                        if (user.IsAdmin)
                        {
                            return RedirectToAction("Admin");
                        }
                        return RedirectToAction("HomePage");
                        
                    }

                }
                // Password did not match with the input email
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            

            return View("Index", model);

        }

        private bool VerifyPassword(string userPassword1, string userPassword2)
        {
            return userPassword1 == userPassword2;
        }

    }
}


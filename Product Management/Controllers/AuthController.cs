using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Constant;
using Product_Management.Data;
using Product_Management.Models.Domain;
using Product_Management.Models.Dto;
using System.Data;

namespace Product_Management.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(SignInManager<UserModel> productDbContext, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = productDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

      
      
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AdminHome", "Admin");
                }
                if (User.IsInRole("User"))
                {
                    return RedirectToAction("UserHomePage", "User");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //login
                var result = await _signInManager.PasswordSignInAsync(model.UserEmail!, model.UserPassword!,model.RememberMe, false);          //remember me not working

                if (result.Succeeded)
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("AdminHome", "Admin");
                    }
                    if (User.IsInRole("User"))
                    {
                        return RedirectToAction("UserHomePage", "User");
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
      

        [HttpPost]
        public async Task<IActionResult> Register(AddUserDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel
                {

                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNo = model.PhoneNo,
                    UserPassword = model.UserPassword,
                    ConfirmPassword = model.ConfirmPassword,
                    CreatedAt = DateTime.Now

                };

                var result = await _userManager.CreateAsync(user, model.UserPassword!);

                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("GetAllUser", "Auth");
                    }
                    await _userManager.AddToRoleAsync(user, Roles.User.ToString()!);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("UserHomePage", "User");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            var users = _userManager.Users;
            return View(users);
        }

        
    }
}


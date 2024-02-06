using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.Domain;
using Product_Management.Models.Dto;

namespace Product_Management.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        public CartController(ProductDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        public IActionResult MyCarts()
        {
           

            var userId = _signInManager.UserManager.GetUserId(User);
            var cartItem = _context.Carts.Include(X => X.Product).Where(x => x.UserId1 == userId).ToList();


            return View(cartItem);
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {

                if (id != null)
                {
                    var existingCart = _context.Carts.FirstOrDefault(x => x.ProductId == id);
                    var user = _signInManager.UserManager.GetUserId(User);


                    if (existingCart != null)
                    {
                        existingCart.CartItems++;
                    }
                    else
                    {
                        var newcart = new Cart()
                        {
                            CartItems = 1,
                            ProductId = id,
                            UserId=12,
                            UserId1 = user
                        };
                        await _context.Carts.AddAsync(newcart);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyCarts");
                }
                return RedirectToAction("UserHomePage", "User");
            }
            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> ReduceQuantity(int id)
        {
            if(id != null)
            {
                var item = _context.Carts.FirstOrDefault(x => x.CartId == id);
                if(item != null)
                {
                    if (item.CartItems > 1)
                    {
                        item.CartItems--;
                        await _context.SaveChangesAsync();

                    }
                    else if (item.CartItems == 1)
                    {
                        
                        return RedirectToAction("RemoveCart", new { id = item.CartId });
                    }
                }
            }
            return RedirectToAction("MyCarts");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveCart(int id)
        {
            if(id != null)
            {
                var item = _context.Carts.FirstOrDefault(x=> x.CartId == id);
                if(item != null)
                {
                    _context.Carts.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("MyCarts");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Migrations;
using Product_Management.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Product_Management.Models.Dto;

namespace Product_Management.Controllers
{
    public class FavouriteController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        public FavouriteController(ProductDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var favouriteProducts = _context.Favourites
                .Where(x => x.UserId == userId)
                .Join(
                    _context.Products,
                    favouriteItem => favouriteItem.ProductId,
                    product => product.ProductId,
                    (favouriteItem, product) => new FavouriteProduct
                    {
                        FavouriteId = favouriteItem.FavouriteId,
                        ProductId = favouriteItem.ProductId,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        ProductImage = product.ProductImage,
                        ProductDescription = product.ProductDescription,
                      
                    }
                )
                .ToList();

            return View(favouriteProducts);
        }

        public async Task<IActionResult> AddToFavourite(int id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (id != 0)
                {

                    var user = _signInManager.UserManager.GetUserId(User);
                    var existItem = _context.Favourites.Where(x => x.UserId == user).FirstOrDefault(x => x.ProductId == id);

                    if (existItem == null)
                    {
                        var item = new Models.Domain.FavouriteModel()
                        {
                            ProductId = id,
                            UserId = user
                        };
                        await _context.Favourites.AddAsync(item);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFavourite(int id)
        {
            if (id != null)
            {
                var item = _context.Favourites.FirstOrDefault(x => x.FavouriteId == id);
                if (item != null)
                {
                    _context.Favourites.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index" , "Favourite");
        }
    }
}

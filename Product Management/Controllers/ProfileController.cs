using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.Domain;

namespace Product_Management.Controllers
{
   
    public class ProfileController : Controller
    {
        private readonly ProductDbContext _context;
        public ProfileController(ProductDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                var profile = new UserModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNo= user.PhoneNo,
                    Role = user.Role,
                    ConfirmPassword = user.ConfirmPassword,
                };
                return View(profile);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

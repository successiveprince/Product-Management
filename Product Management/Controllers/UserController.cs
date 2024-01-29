using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Models.Domain;
using System.Reflection.Metadata.Ecma335;

namespace Product_Management.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductDbContext _context;

        public UserController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult UserHomePage()
        {
            return View();
        }
        
    }

}

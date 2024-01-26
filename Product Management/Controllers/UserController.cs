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
        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Verify(User user)
        //{
        //    var userInDb = _context.Users.FirstOrDefault(u => u.UserEmail == 
        //    if(userInDb != null)
        //    {

        //    })
        //    return Ok(user);
        //}
        
    }

}

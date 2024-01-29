using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;

namespace Product_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProductDbContext _context;
        public AdminController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Admin()
        {
            
            return View();
        }
    }
}

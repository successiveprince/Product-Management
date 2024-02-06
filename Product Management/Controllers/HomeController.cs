using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace Product_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ProductDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy(string myData)
        {
            if(myData == "all") 
            {
                return Json(new { result = "success" });
            }
            int categoryId = _context.Categories.FirstOrDefault(x => x.CategoryName == myData).CategoryId;
            TempData["CategoryId"] = categoryId;
            return Json(new { result = "success" });
           
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

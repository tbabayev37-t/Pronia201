using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Contexts;

namespace MVCProniaTask.Areas.Admin.Controllers;
[Area("Admin")]
public class ShippingController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
          var shipping =   _context.Shippings.ToList();
            return View(shipping);
        }
    }


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Contexts;
using MVCProniaTask.ViewModels;

namespace MVCProniaTask.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _dbContext;
        public HomeController(AppDbContext context)
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            // var sliders = _dbContext.Sliders.ToList();
            HomeViewModel vm = new HomeViewModel
            {
                Sliders = _dbContext.Sliders.ToList(),
                Shippings = _dbContext.Shippings.ToList(),
            };
           return View(vm);
        }
        public IActionResult  CreateShipping()
        {
            return View();
        }
        public IActionResult CreateNewShipping(Shipping shipping)
        {
           
            _dbContext.Shippings.Add(shipping);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}

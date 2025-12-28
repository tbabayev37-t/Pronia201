using Microsoft.AspNetCore.Mvc;

namespace MVCProniaTask.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

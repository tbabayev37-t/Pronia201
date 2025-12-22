using Microsoft.AspNetCore.Mvc;

namespace MVCProniaTask.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

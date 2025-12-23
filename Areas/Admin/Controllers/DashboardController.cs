using Microsoft.AspNetCore.Mvc;

namespace MVCProniaTask.Areas.Admin.Controllers;
[Area("Admin")]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }


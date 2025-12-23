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
    [HttpGet]
        public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
       public IActionResult Create(Shipping shipping)
    {

        _context.Shippings.Add(shipping);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var shipping = _context.Shippings.Find(id);

        if( shipping == null)
        {
            return NotFound();
        }
        _context.Shippings.Remove(shipping);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
 }


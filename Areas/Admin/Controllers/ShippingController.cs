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
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int id)
    {
        var shipping =  _context.Sliders.Find(id);
        if(shipping is null)
        {
            return NotFound();
        }

        return View(shipping);
    }
    [HttpPost]
    public IActionResult Update(Shipping shipping)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }


        var existShipping = _context.Shippings.Find(shipping.Id);
        if(existShipping is null)
        {
            return NotFound();
        }
        existShipping.ImageUrl = shipping.ImageUrl;
        existShipping.Title = shipping.Title;
        existShipping.Description = shipping.Description;

        _context.Shippings.Update(existShipping);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
 }


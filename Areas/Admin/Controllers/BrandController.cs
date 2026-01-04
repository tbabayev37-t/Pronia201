using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Contexts;

namespace MVCProniaTask.Areas.Admin.Controllers;
[Area("Admin")]

public class BrandController(AppDbContext _contex) : Controller
{
    public IActionResult Index()
    {
        var brand = _contex.Brands.ToList();
        return View(brand);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Brand brand)
    {
        if(!ModelState.IsValid)
        {
            return View(brand);
        }
        _contex.Brands.Add(brand);
        _contex.SaveChanges();

        return RedirectToAction(nameof(Index)); 
    }
    public IActionResult Delete(int id)
    {
        var brands = _contex.Brands.Find(id);
        if(brands == null)
        {
            return NotFound();
        }
        _contex.Brands.Remove(brands);
        _contex.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public IActionResult Update(int id)
    {
        var updatebrand = _contex.Brands.Find(id);
        return View(updatebrand);
    }
    [HttpPost]
    public IActionResult Update(Brand brand)
    {
        if(!ModelState.IsValid)
        {
            return View(brand);
        }
        var existBrands = _contex.Brands.Find(brand.Id);
        if(existBrands is null)
        {
            return NotFound();
        }
        existBrands.Name = brand.Name;
        _contex.Brands.Update(existBrands);
        _contex.SaveChanges(); 
        return RedirectToAction(nameof(Index));

    }
}

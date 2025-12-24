using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Contexts;
using NuGet.Protocol.Plugins;

namespace MVCProniaTask.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController(AppDbContext _context) : Controller
{
    public IActionResult Index()
    {
        var products = _context.Products.Include(x => x.Category).ToList();

        return View(products);
    }
    [HttpGet]
    public IActionResult Create()
    {
        SendCategoriesWithViewBag();
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        
        if (!ModelState.IsValid)
        {
            SendCategoriesWithViewBag();
            return View(product);
        }

        var isExistCategory = _context.Categories.Any(x => x.Id == product.CategoryId);
        if (!isExistCategory)
        {
            ModelState.AddModelError("", "Bu kategoriya movcud deyil!");
            SendCategoriesWithViewBag();
            return View(product);
        }
        _context.Products.Add(product);   
        _context.SaveChanges();          

        return RedirectToAction(nameof(Index));
        SendCategoriesWithViewBag();
        return RedirectToAction(nameof(Index));
    }

    private void SendCategoriesWithViewBag()
    {
        var categories = _context.Categories.ToList();
        ViewBag.Categories = categories;
    }

    public IActionResult Update(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return NotFound();
        }

        SendCategoriesWithViewBag(_context);
        return View(product);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(Product product)
    {
       
        if (!ModelState.IsValid)
        {
            SendCategoriesWithViewBag();
            return View(product);
        }
        var existProduct = _context.Products.Find(product.Id);
        if(existProduct is null)
        {
            return NotFound();
        }
        var isExistCategory = _context.Categories.Any(x => x.Id ==product.CategoryId);
        if (!isExistCategory)
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("CategoryId", "Bu category movcud deyil");
            return View(product);
        }

        existProduct.Name = product.Name;
        existProduct.Description = product.Description;
        existProduct.SKU = product.SKU;
        existProduct.CategoryId = product.CategoryId;
        existProduct.Price = product.Price;
        existProduct.MainImage = product.MainImage;
        existProduct.HoverImage = product.HoverImage;

        _context.Products.Update(existProduct);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index)); 
    }
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if(product is null)
        {
            return NotFound();
        }
        _context.Products.Remove(product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));

    }
    private void SendCategoriesWithViewBag(AppDbContext _context)
    {
        var categories = _context.Categories.ToList();
        ViewBag.Categories = categories;
    }
}

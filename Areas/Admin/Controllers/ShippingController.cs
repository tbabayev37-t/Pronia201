using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Contexts;

namespace MVCProniaTask.Areas.Admin.Controllers;

[Area("Admin")]
public class ShippingController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ShippingController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var shipping = _context.Shippings.ToList();
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
        if (ModelState.IsValid == false)
        {
            return View(shipping);
        }
        if (!shipping.Image.ContentType.Contains("image"))
        {
            ModelState.AddModelError("Image", "Yalniz sekil formatinda data daxil edin");
            return View(shipping);
        }
        if(shipping.Image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("Image", "Max 2mb hecminde sekil yukleye bilersiniz");
            return View(shipping);
        }
        if (shipping.Image != null)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + shipping.Image.FileName;
            // string folderPath = @$"{_webHostEnvironment.WebRootPath}\assets\images\website-images\{uniqueFileName}";
            string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", uniqueFileName);
            using FileStream stream = new FileStream(folderPath, FileMode.Create);
            shipping.Image.CopyTo(stream);
            shipping.ImageUrl = uniqueFileName;
        }

        _context.Shippings.Add(shipping);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var shipping = _context.Shippings.Find(id);
        if (shipping == null) return NotFound();

        _context.Shippings.Remove(shipping);
        _context.SaveChanges();

        string folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", shipping.ImageUrl);

        if ( System.IO.File.Exists(folderPath))
            System.IO.File.Delete(folderPath);
            
        

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int id)
    {
        var shipping = _context.Shippings.Find(id);
        if (shipping == null) return NotFound();

        return View(shipping);
    }

    [HttpPost]
    public IActionResult Update(Shipping shipping)
    {
        if (!ModelState.IsValid) return View();

        var existShipping = _context.Shippings.Find(shipping.Id);
        if (existShipping == null) return NotFound();

        existShipping.Title = shipping.Title;
        existShipping.Description = shipping.Description;
        if (shipping.Image != null)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + shipping.Image.FileName;
            string folderPath = @$"{_webHostEnvironment.WebRootPath}\assets\images\website-images\{uniqueFileName}";
            using FileStream stream = new FileStream(folderPath, FileMode.Create);
            shipping.Image.CopyTo(stream);
            existShipping.ImageUrl = uniqueFileName;
        }

        _context.Shippings.Update(existShipping);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}

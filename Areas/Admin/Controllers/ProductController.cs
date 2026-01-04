using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Contexts;
using MVCProniaTask.Helpers;
using MVCProniaTask.Models;
using MVCProniaTask.ViewModels.ProductViewModels;
using NuGet.Protocol.Plugins;

namespace MVCProniaTask.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
{
    public IActionResult Index()
    {
       List<ProductGetVM> vms = _context.Products.Include(x => x.Category).Select(x=>new ProductGetVM()
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            CategoryName = x.Category.Name,
            HoverImage = x.HoverImage,
            Price = x.Price,
            SKU = x.SKU,
            MainImage = x.MainImage

        }).ToList();

        return View(vms);
    }
    [HttpGet]
    public IActionResult Create()
    {
        SendCategoriesWithViewBag();
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductCreateVM product)
    {

        if (!ModelState.IsValid)
        {
            SendCategoriesWithViewBag();
            return View(product);
        }
        if (!product.MainImage1.CheckType("image"))
        {
            ModelState.AddModelError("MainImage1", "Yalniz sekil formatinda data daxil edin");
            return View(product);
        }
        if (!product.MainImage1.CheckSize(2))
        {
            ModelState.AddModelError("MainImage1", "Max 2mb hecminde sekil yukleye bilersiniz");
            return View(product);
        }

        if (!product.HoverImage2.CheckType("image"))
        {
            ModelState.AddModelError("HoverImage2", "Yalniz sekil formatinda data daxil edin");
            return View(product);
        }
        if (!product.HoverImage2.CheckSize(2))
        {
            ModelState.AddModelError("HoverImage2", "Max 2mb hecminde sekil yukleye bilersiniz");
            return View(product);
        }
        var isExistCategory = _context.Categories.Any(x => x.Id == product.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("", "Bu kategoriya movcud deyil!");
            SendCategoriesWithViewBag();
            return View(product);
        }
        string folderpath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
        string mainImageUniqueName = product.MainImage1.SaveFile(folderpath);
        string hoverImageUniqueName = product.HoverImage2.SaveFile(folderpath);




        Product product1 = new Product()
        {
            Name = product.Name, 
            Description = product.Description,
            SKU = product.SKU,
            Price =product.Price,
            CategoryId = product.CategoryId,
            MainImage = mainImageUniqueName,
            HoverImage = hoverImageUniqueName
        };
        product1.ProductImages = new List<ProductImage>();
        if(product1.ProductImages is not null)
        {
            foreach (var image in product.Images)
            {
                if (!image.CheckType("image") || !image.CheckSize(2))
                    continue;

                string imagename = image.SaveFile(folderpath);
                product1.ProductImages.Add(new ProductImage
                {
                    ImageUrl = imagename
                });
            }
           
        }
        _context.Products.Add(product1);
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

        ProductUpdateVM vm = new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            Price = product.Price,
            SKU = product.SKU
        };
        return View(vm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(ProductUpdateVM product)
    {

        if (!ModelState.IsValid)
        {
            SendCategoriesWithViewBag();
            return View(product);
        }
        var existProduct = _context.Products.Find(product.Id);
        if (existProduct is null)
        {
            return NotFound();
        }
        var isExistCategory = _context.Categories.Any(x => x.Id == product.CategoryId);
        if (!isExistCategory)
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("CategoryId", "Bu category movcud deyil");
            return View(product);
        }

        if (!product.MainImage?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("MainImage1", "Yalniz sekil formatinda data daxil edin");
            return View(product);
        }
        if (!product.MainImage?.CheckSize(2)??false)
        {
            ModelState.AddModelError("MainImage1", "Max 2mb hecminde sekil yukleye bilersiniz");
            return View(product);
        }

        if (!product.HoverImage?.CheckType("image")??false)
        {
            ModelState.AddModelError("HoverImage2", "Yalniz sekil formatinda data daxil edin");
            return View(product);
        }
        if (!product.HoverImage?.CheckSize(2)??false)
        {
            ModelState.AddModelError("HoverImage2", "Max 2mb hecminde sekil yukleye bilersiniz");
            return View(product);
        }
        

        if (!isExistCategory)
        {
            ModelState.AddModelError("", "Bu kategoriya movcud deyil!");
            SendCategoriesWithViewBag();
            return View(product);
        }

        existProduct.Name = product.Name;
        existProduct.Description = product.Description;
        existProduct.SKU = product.SKU;
        existProduct.CategoryId = product.CategoryId;
        existProduct.Price = product.Price;
        /* existProduct.MainImage = product.MainImage;
         existProduct.HoverImage = product.HoverImage;*/
        string folderpath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");

        if (product.MainImage is { })
        {
            string newMainImageName = product.MainImage.SaveFile(folderpath);
            if (System.IO.File.Exists(Path.Combine(folderpath, existProduct.MainImage)))
                System.IO.File.Delete(Path.Combine(folderpath, existProduct.MainImage));

            existProduct.MainImage = newMainImageName;
        }
        if(product.HoverImage is { })
        {
            string newHoverImageName = product.HoverImage.SaveFile(folderpath);
            if (System.IO.File.Exists(Path.Combine(folderpath, existProduct.HoverImage)))
                System.IO.File.Delete(Path.Combine(folderpath, existProduct.HoverImage));

            existProduct.HoverImage = newHoverImageName;
        }

        _context.Products.Update(existProduct);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product is null)
        {
            return NotFound();
        }
        _context.Products.Remove(product);
        _context.SaveChanges();
        string folderpath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");
        if (System.IO.File.Exists(Path.Combine(folderpath, product.MainImage)))
            System.IO.File.Delete(Path.Combine(folderpath, product.MainImage));

        if (System.IO.File.Exists(Path.Combine(folderpath, product.HoverImage)))
            System.IO.File.Delete(Path.Combine(folderpath, product.HoverImage));

        return RedirectToAction(nameof(Index));

    }
    public IActionResult Detail(int id)
    {
        var product = _context.Products.Select(x=>new ProductGetVM()
        {
            Id=x.Id,
            CategoryName=x.Category.Name,
            Description=x.Description,
            Name=x.Name,
            HoverImage=x.HoverImage,
            MainImage=x.MainImage,
            Price=x.Price,
            SKU=x.SKU,
            TagNames=x.ProductTags.Select(x=>x.Tag.name).ToList(),
            ImageUrls = x.ProductImages.Select(x=>x.ImageUrl).ToList()

        }).FirstOrDefault(x=>x.Id == id);
        if(product is null)
        {
            return NotFound();
        }
        return View(product);
    }
    private void SendCategoriesWithViewBag(AppDbContext _context)
    {
        var categories = _context.Categories.ToList();
        ViewBag.Categories = categories;
    }
}
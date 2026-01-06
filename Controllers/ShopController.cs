using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Contexts;

namespace MVCProniaTask.Controllers
{
    public class ShopController(AppDbContext _contex) : Controller
    {
        [Authorize]
        public IActionResult Index()
        {

            var products = _contex.Products.Select(product=>new ProductGetVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.Category.Name,
                HoverImage = product.HoverImage,
                Price = product.Price,
                SKU = product.SKU,
                MainImage = product.MainImage
            }).ToList();
            return View(products);
        }
    }
}

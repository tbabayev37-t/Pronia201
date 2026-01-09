using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Contexts;
using MVCProniaTask.Models.Basket;
using System.Security.Claims;

namespace MVCProniaTask.Controllers
{
    public class BasketController(AppDbContext _context) : Controller
    {
        [Authorize]
        public async Task<IActionResult> AddToBasket(int productId)
        {
            var isExistProduct = await _context.Products.AnyAsync(x=>x.Id == productId);

            if (!isExistProduct)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isExistUser = await _context.Users.AnyAsync(x=>x.Id == userId);
            if(!isExistUser) { BadRequest(); }
            var ExistBasketItem = await _context.BasketItems.FirstOrDefaultAsync(x=>x.AppUserId == userId && x.ProductId==productId);
            if (ExistBasketItem is { })
            {
                ExistBasketItem.Count++;
                _context.Update(ExistBasketItem);
            }
            else
            {
                BasketItem basketItem = new()
                {
                    ProductId = productId,
                    Count = 1,
                    AppUserId = userId!
                };
                await _context.BasketItems.AddAsync(basketItem);
                await _context.SaveChangesAsync();
            }
            

            

            TempData["Success message"] = "Product successfully added";
            return RedirectToAction("Index","Shop");
        }
    }
}

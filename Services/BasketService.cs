using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Abstractions;
using MVCProniaTask.Contexts;
using MVCProniaTask.Models.Basket;
using System.Security.Claims;

namespace MVCProniaTask.Services
{
    public class BasketService(IHttpContextAccessor _accessor, AppDbContext _context):IBasketService
    {
        public async Task<List<BasketItem>>  GetBasketItemsAsync()
        {
            var userId = _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var isExistUser = await _context.Users.AnyAsync(x=>x.Id == userId);
            if (!isExistUser)
            {
                return [];
            }
 
            var basketItems = await _context.BasketItems.Include(x=>x.Product).Where(x=> x.AppUserId == userId).ToListAsync();    

            return basketItems;
        }
    }
}

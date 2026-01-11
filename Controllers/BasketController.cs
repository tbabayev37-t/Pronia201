using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Abstractions;
using MVCProniaTask.Contexts;
using MVCProniaTask.Models.Basket;
using System.Security.Claims;

namespace MVCProniaTask.Controllers;
[Authorize]
public class BasketController(AppDbContext _context, IBasketService _basketService) : Controller
{
    public async Task<IActionResult>  Index()
    {
        var basketItems = await _basketService.GetBasketItemsAsync();
        return View(basketItems);
    }
    
    public async Task<IActionResult> AddToBasket(int productId)
    {
        var isExistProduct = await _context.Products.AnyAsync(x=>x.Id == productId);

        if (!isExistProduct)
        {
            return NotFound();
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var isExistUser = await _context.Users.AnyAsync(x=>x.Id == userId);
        if(!isExistUser) { 
           return BadRequest(); }
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
    public async Task<IActionResult> RemoveFromBasket(int productId)
    {
        var isExistProduct = await _context.Products.AnyAsync(x => x.Id == productId);

        if (!isExistProduct)
        {
            return NotFound();
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
        if (!isExistUser) 
         return BadRequest(); 

        var existbasketItem  = await _context.BasketItems.FirstOrDefaultAsync(x=>x.AppUserId == userId && x.ProductId == productId);
        if (existbasketItem is null)
        {
            return NotFound();
        }
        _context.BasketItems.Remove(existbasketItem);
        await _context.SaveChangesAsync();
        var returnUrl =  Request.Headers["Referer"];
        if(!string.IsNullOrWhiteSpace(returnUrl))
        {
            return Redirect(returnUrl!);
        }
        return RedirectToAction("Index", "Shop");
    }
    public async Task<IActionResult>  DecreaseBasketItemCount(int productId)
    {
        var isExistProduct = await _context.Products.AnyAsync(x => x.Id == productId);

        if (!isExistProduct)
        {
            return NotFound();
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var isExistUser = await _context.Users.AnyAsync(x => x.Id == userId);
        if (!isExistUser)
            return BadRequest();

        var existbasketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.AppUserId == userId && x.ProductId == productId);
        if (existbasketItem is null)
        {
            return NotFound();
        }
        if (existbasketItem.Count > 1)
            existbasketItem.Count--;

        _context.BasketItems.Update(existbasketItem);
        await _context.SaveChangesAsync();
        var returnUrl = Request.Headers["Referer"];
        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            return Redirect(returnUrl!);
        }
        return RedirectToAction("Index", "Shop");
    }
}

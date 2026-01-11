using MVCProniaTask.Models.Basket;

namespace MVCProniaTask.Abstractions
{
    public interface IBasketService
    {
        Task<List<BasketItem>> GetBasketItemsAsync();
    }
}

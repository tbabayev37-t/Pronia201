using MVCProniaTask.Models.Command;

namespace MVCProniaTask.Models.Basket
{
    public class BasketItem:BaseEntity
    {
        public int ProductId {  get; set; }
        public Product Product { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
        
        public int Count { get; set; }

    }
}

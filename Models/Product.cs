using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Models.Basket;
using System.ComponentModel.DataAnnotations;

namespace MVCProniaTask.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
       
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
       
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
        public string MainImage { get; set; }
       
        public string HoverImage { get; set; }
       
        public double Rating { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; } = [];
        public ICollection<ProductImage> ProductImages { get; set; } = [];
        public ICollection<BasketItem> BasketItems { get; set; } = [];
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}

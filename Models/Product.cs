using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVCProniaTask.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required]
        public string MainImage { get; set; }
        [Required]
        public string HoverImage { get; set; }
        [Range(0, 5)]
        public double Rating { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; } = [];
        public ICollection<ProductImage> ProductImages { get; set; } = [];
    }
}

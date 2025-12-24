using Microsoft.EntityFrameworkCore;
using MVCProniaTask.Models.Command;
using System.ComponentModel.DataAnnotations;

namespace MVCProniaTask.Models
{
    public class Product:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        [Required]
        public string MainImage { get; set; }
        [Required]
        public string HoverImage { get; set; }

    }
}

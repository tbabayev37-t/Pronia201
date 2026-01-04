using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVCProniaTask.ViewModels.ProductViewModels
{
    public class ProductCreateVM
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
        [Required]
        public IFormFile MainImage1 { get; set; }
        [Required]
        public IFormFile HoverImage2 { get; set; }
        public List<IFormFile>? Images { get; set; }

    }
}

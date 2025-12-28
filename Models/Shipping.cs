using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCProniaTask.Models
{
    public class Shipping
    {
        
        public int Id { get; set; }
       
        [MaxLength(512), MinLength(3)]
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image {  get; set; }
        
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}

using MVCProniaTask.Models.Command;
using System.ComponentModel.DataAnnotations;

namespace MVCProniaTask.Models
{
    public class Shipping:BaseEntity
    {
        [Required]
        [MaxLength(512), MinLength(3)]
        public string ImageUrl { get; set; }
        
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

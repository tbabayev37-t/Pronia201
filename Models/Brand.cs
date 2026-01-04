using MVCProniaTask.Models.Command;
using System.ComponentModel.DataAnnotations;

namespace MVCProniaTask.Models
{
    public class Brand:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

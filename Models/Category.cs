using System.ComponentModel.DataAnnotations;

namespace MVCProniaTask.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
    }
}
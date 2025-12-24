using MVCProniaTask.Models.Command;
using System.ComponentModel.DataAnnotations;

public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
    [Required]
    [MaxLength(512)]
    public string ImageUrl { get; set; }
}
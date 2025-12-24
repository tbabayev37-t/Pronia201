using MVCProniaTask.Models.Command;
using System.ComponentModel.DataAnnotations;

public class Category : BaseEntity
{
    [Required]
    [MaxLength(256)]
    public string Name { get; set; }
}

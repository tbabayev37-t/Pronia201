using System.ComponentModel.DataAnnotations;

public class ProductGetVM()
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public string CategoryName { get; set; }
    public string MainImage { get; set; }
    public string HoverImage { get; set; }
    [Range(0, 5)]
    public double Rating { get; set; } = 3;
    public List<string> TagNames { get; set; }
    public List<string> ImageUrls { get; set; }


}
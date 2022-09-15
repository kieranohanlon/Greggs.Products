namespace Greggs.Products.Models;

public class Product
{
    public string? Name { get; set; }
    public decimal PriceInPounds { get; set; }
    public decimal Price { get; set; }
    public string? PriceIsInCurrencyCode { get; set; }
}
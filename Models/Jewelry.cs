namespace WebApi.Models;
public class Jewelry
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public string? Category { get; set; }
    public int UserId { get; set; }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DbChart.Web.Database;

public sealed class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [MaxLength(100)]
    public required string ProductName { get; set; }
    
    [Precision(9, 2)]
    public required decimal Price { get; set; }

    //[RelationshipDescription("is purchased via")]
    public List<OrderItem> OrderItems { get; set; } = [];
}
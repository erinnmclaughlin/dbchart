namespace DbChart.Web.Database;

public sealed class Invoice
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    
    [RelationshipDescription("is issued to")]
    public Customer Customer { get; set; } = null!;

    [RelationshipDescription("can cover")]
    public List<Order> Orders { get; set; } = [];
}
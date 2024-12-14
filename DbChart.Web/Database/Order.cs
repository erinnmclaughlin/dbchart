namespace DbChart.Web.Database;

public sealed class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Guid InvoiceId { get; set; }
    public DateTimeOffset PlacedOn { get; init; } = DateTime.UtcNow;
    
    [RelationshipDescription("is placed by")]
    public Customer Customer { get; set; } = null!;
    
    [RelationshipDescription("is covered by")]
    public Invoice Invoice { get; set; } = null!;

    [RelationshipDescription("includes")]
    public List<OrderItem> OrderItems { get; set; } = [];
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DbChart.Web.Database;

[Comment("A customer may be an individual or a business entity.")]
[Index(nameof(EmailAddress), IsUnique = true)]
public sealed class Customer
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Comment("All customers must have a unique email address.")]
    [EmailAddress, MaxLength(255)]
    public required string EmailAddress { get; set; }

    [RelationshipDescription("is liable for")]
    public List<Invoice> Invoices { get; set; } = [];
    
    [RelationshipDescription("places")]
    public List<Order> Orders { get; set; } = [];
}
using Microsoft.EntityFrameworkCore;

namespace DbChart.Web.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(builder =>
        {
            builder.HasMany(x => x.Orders).WithOne(x => x.Customer).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Invoices).WithOne(x => x.Customer).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(builder =>
        {
            builder.HasOne(x => x.Invoice).WithMany(x => x.Orders).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasMany(x => x.OrderItems).WithOne(x => x.Product).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
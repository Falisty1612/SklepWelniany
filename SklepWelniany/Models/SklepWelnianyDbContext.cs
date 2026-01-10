using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics; // <--- 1. DODAJ TEN USING
using SklepWelniany.Models;

public class SklepWelnianyDbContext : IdentityDbContext<IdentityUser>
{
    public SklepWelnianyDbContext(DbContextOptions<SklepWelnianyDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

    //(Ignorowanie błędu o migracjach) ---
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // To polecenie każe EF Core ignorować ostrzeżenie o "wiszących zmianach" i uruchomić się mimo to
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }
    // ------------------------------------------------------------

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ostrzeżenie o cenie (Price)
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Cart>()
            .HasKey(c => c.Id);

        builder.Entity<Cart>()
            .HasMany(c => c.Items)
            .WithOne(i => i.Cart)
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartItem>()
            .HasKey(ci => ci.Id);

        builder.Entity<ProductImage>()
            .HasKey(pi => pi.Id);

        builder.Entity<ProductImage>()
            .HasOne(p => p.Product)
            .WithMany(p => p.Images)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
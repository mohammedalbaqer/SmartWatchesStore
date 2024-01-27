using Microsoft.EntityFrameworkCore;
using SmartWatchesStore.Models;

namespace SmartWatchesStore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
    }
}

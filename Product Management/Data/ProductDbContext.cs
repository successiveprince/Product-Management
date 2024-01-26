using Microsoft.EntityFrameworkCore;
using Product_Management.Models.Domain;

namespace Product_Management.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}

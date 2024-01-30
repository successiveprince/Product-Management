using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product_Management.Configuration;
using Product_Management.Models.Domain;

namespace Product_Management.Data
{
    public class ProductDbContext : IdentityDbContext<UserModel>
    {
        public ProductDbContext(DbContextOptions options): base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

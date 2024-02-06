using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product_Management.Models.Domain;

namespace Product_Management.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.Property(x => x.ProductId).IsUnicode(true).ValueGeneratedOnAdd();
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId)
                .IsRequired(false);

            builder.HasOne(x => x.Cart)
                .WithOne(x => x.Product)
                .HasPrincipalKey<Product>(x => x.ProductId)
                .IsRequired(false);
        }
    }
}

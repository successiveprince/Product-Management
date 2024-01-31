using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product_Management.Models.Domain;

namespace Product_Management.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.Property(x => x.CategoryId).IsUnicode(true).ValueGeneratedOnAdd();
            builder.HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasPrincipalKey(x => x.CategoryId)
                .HasForeignKey(x => x.ProductCategoryId);
        }
    }
}

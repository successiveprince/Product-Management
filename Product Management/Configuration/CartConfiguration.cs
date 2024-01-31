using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Product_Management.Models.Domain;

namespace Product_Management.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");
            builder.Property(z => z.CartId).IsUnicode(true).ValueGeneratedOnAdd();


        }
    }
}

using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Mappings
{
    internal class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("PRODUCTS", "CIA");

            builder.Property(p => p.ID)
                .HasColumnName("ID");

            builder.Property(p => p.Name)
                .HasColumnName("NAME");

            builder.Property(p => p.Description)
                .HasColumnName("DESCRIPTION");

            builder.Property(p => p.Price)
                .HasColumnName("PRICE");

            builder.Property(p => p.ImageName)
                .HasColumnName("IMAGENAME");
        }
    }
}

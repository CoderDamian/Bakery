using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataPersistence.Mappings
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USERS", "CIA");

            builder.Property(p => p.Id)
                .HasColumnName("ID");

            builder.Property(p => p.Name)
                .HasColumnName("USERNAME");

            builder.Property(p => p.Password)
                .HasColumnName("PASSWORD");
        }
    }
}

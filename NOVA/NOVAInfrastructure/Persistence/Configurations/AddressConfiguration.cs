using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NOVA.Domain.Entities;

namespace NOVAInfrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.State)
                .HasMaxLength(100);

            builder.Property(a => a.ZipCode)
                .HasMaxLength(20);

            //builder.Property(a => a.IsDefault)
            //    .HasDefaultValue(false);
        }
    }
}

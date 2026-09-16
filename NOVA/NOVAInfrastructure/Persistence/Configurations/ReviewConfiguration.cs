using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NOVA.Domain.Entities;

namespace NOVAInfrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .HasMaxLength(1000);

            builder.Property(r => r.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(r => new { r.UserId, r.ProductId })
                .IsUnique();
        }
    }
}

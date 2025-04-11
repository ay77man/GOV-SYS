using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using E_Government.Core.Domain.Entities;

namespace E_Government.Infrastructure._Data.Configuration
{
    public class MeterConfiguration : IEntityTypeConfiguration<Meter>
    {
        public void Configure(EntityTypeBuilder<Meter> builder)
        {
            builder.ToTable("Meters", "Billing");

            // Configure primary key with identity
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .UseIdentityColumn(); // This enables auto-increment

            builder.Property(m => m.MeterNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(m => m.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasOne(m => m.Customer)
                .WithMany(c => c.Meters)
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(m => m.MeterNumber)
                .IsUnique();
        }
    }
}
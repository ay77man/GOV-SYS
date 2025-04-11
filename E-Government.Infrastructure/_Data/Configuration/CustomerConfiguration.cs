using E_Government.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", "Billing");

        builder.HasKey(c => c.Id);

        // Property configurations
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.NationalId)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.AccountNumber) // New configuration
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Category)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relationships
        builder.HasMany(c => c.Payments)
            .WithOne(p => p.Customer)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Meters)
            .WithOne(m => m.Customer)
            .HasForeignKey(m => m.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Bills)
            .WithOne(b => b.Customer)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(c => c.NationalId)
            .IsUnique()
            .HasDatabaseName("IX_Customers_NationalId");

        builder.HasIndex(c => c.Phone)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Phone");

        
        builder.Property(c => c.Email)
                   .HasMaxLength(100); // Remove IsRequired() if emails can be empty

        // This creates a unique index only for non-null, non-empty emails
        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Email")
            .HasFilter("[Email] IS NOT NULL AND [Email] <> ''");

        builder.HasIndex(c => c.AccountNumber) // New index
            .IsUnique()
            .HasDatabaseName("IX_Customers_AccountNumber");

        builder.HasIndex(c => c.Status)
            .HasDatabaseName("IX_Customers_Status");
    }
}
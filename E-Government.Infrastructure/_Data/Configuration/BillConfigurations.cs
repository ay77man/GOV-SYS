using E_Government.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Government.Infrastructure._Data.Configuration
{
    public class BillConfigurations : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bills", "Billing");
            builder.HasKey(b => b.Id);

            // Properties configuration
            builder.Property(b => b.BillNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.PaymentId)
         .IsRequired(false); // جعل الحقل غير مطلوب

            builder.Property(e => e.PdfUrl)
                .IsRequired(false); // جعل الحقل غير مطلوب

            builder.Property(e => e.StripePaymentId)
                .IsRequired(false);

            builder.Property(b => b.IssueDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.DueDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(b => b.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            // Meter readings configuration
            builder.Property(b => b.PreviousReading)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.CurrentReading)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Payment related properties
            builder.Property(b => b.PaymentId)
                .HasMaxLength(100);

            builder.Property(b => b.StripePaymentId)
                .HasMaxLength(100);

            builder.Property(b => b.PdfUrl)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(b => b.Meter)
                .WithMany(m => m.Bills)
                .HasForeignKey(b => b.MeterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Customer)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Payment)
                .WithOne(p => p.Bill)
                .HasForeignKey<Payment>(p => p.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(b => b.BillNumber).IsUnique();
            builder.HasIndex(b => b.Status);
            builder.HasIndex(b => b.DueDate);
            builder.HasIndex(b => b.CustomerId);
            builder.HasIndex(b => b.MeterId);
            builder.HasIndex(b => b.PaymentId);
            builder.HasIndex(b => b.StripePaymentId);
        }
    }
}
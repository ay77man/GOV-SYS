using E_Government.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", "Billing");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(p => p.PaymentDate).IsRequired();
        builder.Property(p => p.TransactionId).IsRequired().HasMaxLength(100);

        // علاقة العميل
        builder.HasOne(p => p.Customer)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        // علاقة الفاتورة
        builder.HasOne(p => p.Bill)
            .WithOne(b => b.Payment)
            .HasForeignKey<Payment>(p => p.BillId)
            .OnDelete(DeleteBehavior.ClientNoAction);

        builder.HasIndex(p => p.TransactionId).IsUnique();
    }
}
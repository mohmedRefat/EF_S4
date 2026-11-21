
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.TransactionNumber);

        builder.Property(t => t.TransactionNumber)
            .ValueGeneratedOnAdd();

        builder.Property(t => t.TransactionDate)
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(t => t.Note)
            .HasMaxLength(300);

        builder.Property(t => t.TransactionType)
            .IsRequired();

        builder.Property(t => t.AccountNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountNumber)
            .IsRequired();
    }
}

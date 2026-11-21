
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.AccountNumber);

        builder.Property(a => a.AccountNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.CurrentBalance)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(a => a.AccountType)
            .IsRequired();

        builder.Property(a => a.OpeningDate)
            .IsRequired();

        builder.Property(a => a.BranchCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasOne(a => a.Branch)
            .WithMany(b => b.Accounts)
            .HasForeignKey(a => a.BranchCode)
            .IsRequired();

        builder.HasMany(a => a.CustomerAccounts)
            .WithOne(ca => ca.Account)
            .HasForeignKey(ca => ca.AccountNumber)
            .IsRequired();

        builder.HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountNumber)
            .IsRequired();
    }
}

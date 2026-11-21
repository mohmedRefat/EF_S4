
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class CustomerAccountConfiguration : IEntityTypeConfiguration<CustomerAccount>
{
    public void Configure(EntityTypeBuilder<CustomerAccount> builder)
    {
        builder.HasKey(ca => new
        {
            ca.CustomerId,
            ca.AccountNumber
        });

        builder.Property(ca => ca.AccountNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ca => ca.OwnershipType)
            .IsRequired();

        builder.Property(ca => ca.OwnershipStartDate)
            .IsRequired();

        builder.Property(ca => ca.AccountStatus)
            .IsRequired();

        builder.HasOne(ca => ca.Customer)
            .WithMany(c => c.CustomerAccounts)
            .HasForeignKey(ca => ca.CustomerId);

        builder.HasOne(ca => ca.Account)
            .WithMany(a => a.CustomerAccounts)
            .HasForeignKey(ca => ca.AccountNumber);
    }
}

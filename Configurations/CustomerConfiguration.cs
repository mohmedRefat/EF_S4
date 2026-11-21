
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.NationalId)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.NationalId)
            .IsUnique();

        builder.Property(c => c.DateOfBirth)
            .IsRequired();

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.CustomerType)
            .IsRequired();

        builder.HasMany(c => c.CustomerAccounts)
            .WithOne(ca => ca.Customer)
            .HasForeignKey(ca => ca.CustomerId)
            .IsRequired();
    }
}

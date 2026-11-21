using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.HasKey(b => b.Code);

        builder.Property(b => b.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(b => b.Manager)
            .WithOne(m => m.Branch)
            .HasForeignKey<Manager>(m => m.BranchCode)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Accounts)
            .WithOne(a => a.Branch)
            .HasForeignKey(a => a.BranchCode)
            .IsRequired();

        builder.HasData(
            new Branch
            {
                Code = "CAI-01",
                Name = "Cairo Main Branch",
                Address = "26 July Street, Downtown, Cairo",
                PhoneNumber = "0225781234"
            },
            new Branch
            {
                Code = "ALX-01",
                Name = "Alexandria Branch",
                Address = "Corniche Road, Smouha, Alexandria",
                PhoneNumber = "0345678901"
            },
            new Branch
            {
                Code = "GIZ-01",
                Name = "Giza Branch",
                Address = "Pyramids Street, Giza",
                PhoneNumber = "0233344556"
            });
    }
}
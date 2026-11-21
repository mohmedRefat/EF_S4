using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.HireDate)
            .IsRequired();

        builder.Property(m => m.BranchCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasData(
            new Manager
            {
                Id = 1,
                FullName = "Khaled Mahmoud",
                Email = "khaled.mahmoud@nationalbank.com",
                PhoneNumber = "01001112233",
                HireDate = new DateTime(2015, 3, 1),
                BranchCode = "CAI-01"
            },
            new Manager
            {
                Id = 2,
                FullName = "Sara Hassan",
                Email = "sara.hassan@nationalbank.com",
                PhoneNumber = "01112223344",
                HireDate = new DateTime(2018, 9, 15),
                BranchCode = "ALX-01"
            },
            new Manager
            {
                Id = 3,
                FullName = "Omar Fathy",
                Email = "omar.fathy@nationalbank.com",
                PhoneNumber = "01223334455",
                HireDate = new DateTime(2020, 1, 10),
                BranchCode = "GIZ-01"
            });
    }
}
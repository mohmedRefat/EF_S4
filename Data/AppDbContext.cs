
using Microsoft.EntityFrameworkCore;


public class AppDbContext : DbContext
{
    public DbSet<Branch> Branches { get; set; }

    public DbSet<Manager> Managers { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<CustomerAccount> CustomerAccounts { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString =
                Environment.GetEnvironmentVariable("DB_CONNECTION")
                ?? throw new Exception("Database connection string is missing.");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly
        );

        base.OnModelCreating(modelBuilder);
    }
}

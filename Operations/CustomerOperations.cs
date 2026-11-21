using System.Globalization;
using Microsoft.EntityFrameworkCore;

public static class CustomerOperations
{
    public static void AddCustomer(AppDbContext context)
    {
        Console.WriteLine("--- Add New Customer ---");

        string fullName = Input.ReadText("Full Name      : ", 100);

        string nationalId = Input.ReadDigits("National ID    : ", 10, 20);
        if (context.Customers.Any(c => c.NationalId == nationalId))
        {
            Output.PrintError("A customer with this National ID already exists.");
            return;
        }

        DateTime dateOfBirth = Input.ReadDate("Date of Birth  : (yyyy-MM-dd) ");
        string email = Input.ReadEmail("Email          : ");
        string phone = Input.ReadDigits("Phone          : ", 7, 15);
        string address = Input.ReadText("Address        : ", 200);

        Console.WriteLine("Customer Type:");
        Console.WriteLine("    1) Individual");
        Console.WriteLine("    2) Business");
        int typeChoice = Input.ReadChoice("  Choice: ", 2);

        Customer customer = new Customer
        {
            FullName = fullName,
            NationalId = nationalId,
            DateOfBirth = dateOfBirth,
            Email = email,
            PhoneNumber = phone,
            Address = address,
            CustomerType = typeChoice == 1 ? CustomerType.Individual : CustomerType.Business
        };

        context.Customers.Add(customer);
        context.SaveChanges();

        Console.WriteLine();
        Output.PrintSuccess("Customer created successfully. CustomerId = " + customer.Id);
    }

    public static void ListCustomers(AppDbContext context)
    {
        Console.WriteLine("--- All Customers ---");
        Console.WriteLine();

        List<Customer> customers = context.Customers
            .Include(c => c.CustomerAccounts)
                .ThenInclude(ca => ca.Account)
                    .ThenInclude(a => a.Branch)
            .OrderBy(c => c.Id)
            .ToList();

        if (customers.Count == 0)
        {
            Console.WriteLine("  (no customers yet)");
            return;
        }

        foreach (Customer customer in customers)
        {
            Console.WriteLine("  #" + customer.Id + " " + customer.FullName + " (" + customer.CustomerType + ")");

            if (customer.CustomerAccounts.Count == 0)
            {
                Console.WriteLine("      (no accounts)");
                continue;
            }

            foreach (CustomerAccount link in customer.CustomerAccounts)
            {
                Account account = link.Account;
                string balance = account.CurrentBalance.ToString("N2", CultureInfo.InvariantCulture);

                Console.WriteLine(
                    "      " + account.AccountNumber.PadRight(10) + " " +
                    account.AccountType.ToString().PadRight(9) + " " +
                    "Balance: " + balance.PadLeft(12) + "   " +
                    link.OwnershipType.ToString().PadRight(8) + " " +
                    link.AccountStatus.ToString().PadRight(7) + " " +
                    "@ " + account.Branch.Name);
            }
        }
    }
}
internal class Program
{
    static void Main(string[] args)
    {
        using AppDbContext context = new AppDbContext();

        while (true)
        {
            ShowMenu();
            string? choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        CustomerOperations.AddCustomer(context);
                        break;
                    case "2":
                        AccountOperations.OpenAccount(context);
                        break;
                    case "3":
                        AccountOperations.UpdateAccountStatus(context);
                        break;
                    case "4":
                        AccountOperations.RemoveAccount(context);
                        break;
                    case "5":
                        CustomerOperations.ListCustomers(context);
                        break;
                    case "0":
                        Console.WriteLine("Closing connection... Goodbye!");
                        return;
                    default:
                        Output.PrintError("Invalid choice. Please enter a number from 0 to 5.");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.ChangeTracker.Clear();
                Output.PrintError("Something went wrong: " + ex.GetBaseException().Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("    National Bank - Management");
        Console.WriteLine("========================================");
        Console.WriteLine("  1) Add a new Customer");
        Console.WriteLine("  2) Open a new Account for a Customer");
        Console.WriteLine("  3) Update Account Status (Active / Closed)");
        Console.WriteLine("  4) Remove an Account from a Customer");
        Console.WriteLine("  5) List all Customers (with accounts)");
        Console.WriteLine("  0) Exit");
        Console.WriteLine("----------------------------------------");
        Console.Write("  Enter choice: ");
    }
}
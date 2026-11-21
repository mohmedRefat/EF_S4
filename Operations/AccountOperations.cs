using Microsoft.EntityFrameworkCore;

public static class AccountOperations
{
    public static void OpenAccount(AppDbContext context)
    {
        Console.WriteLine("--- Open New Account ---");

        string accountNumber = Input.ReadText("Account Number : ", 20);

        Console.WriteLine("Account Type:");
        Console.WriteLine("    1) Savings");
        Console.WriteLine("    2) Current");
        Console.WriteLine("    3) Business");
        int typeChoice = Input.ReadChoice("  Choice: ", 3);
        AccountType accountType = (AccountType)(typeChoice - 1);

        string branchCode = Input.ReadText("Branch Code    : ", 10).ToUpper();
        int customerId = Input.ReadInt("Customer Id    : ");

        Console.WriteLine("Ownership Role:");
        Console.WriteLine("    1) Primary");
        Console.WriteLine("    2) CoHolder");
        int roleChoice = Input.ReadChoice("  Choice: ", 2);
        OwnershipType role = (OwnershipType)(roleChoice - 1);

        Output.PrintInfo("Validating branch '" + branchCode + "' and customer #" + customerId + "...");

        Branch? branch = context.Branches.FirstOrDefault(b => b.Code == branchCode);
        if (branch == null)
        {
            Output.PrintError("Branch '" + branchCode + "' does not exist.");
            return;
        }

        Customer? customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            Output.PrintError("Customer #" + customerId + " does not exist.");
            return;
        }

        Account? account = context.Accounts
            .Include(a => a.CustomerAccounts)
            .FirstOrDefault(a => a.AccountNumber == accountNumber);

        if (account == null)
        {
            if (role != OwnershipType.Primary)
            {
                Output.PrintError("A new account must be opened with a Primary owner.");
                return;
            }

            account = new Account
            {
                AccountNumber = accountNumber,
                AccountType = accountType,
                BranchCode = branch.Code,
                CurrentBalance = 0,
                OpeningDate = DateTime.Now
            };
            context.Accounts.Add(account);
        }
        else
        {
            if (account.BranchCode != branch.Code)
            {
                Output.PrintError("Account '" + accountNumber + "' belongs to branch '" + account.BranchCode + "'.");
                return;
            }

            if (account.CustomerAccounts.Any(ca => ca.CustomerId == customerId))
            {
                Output.PrintError("This customer is already linked to this account.");
                return;
            }

            if (role == OwnershipType.Primary && account.CustomerAccounts.Any(ca => ca.OwnershipType == OwnershipType.Primary))
            {
                Output.PrintError("This account already has a Primary owner. Choose CoHolder.");
                return;
            }
        }

        CustomerAccount link = new CustomerAccount
        {
            CustomerId = customerId,
            AccountNumber = accountNumber,
            OwnershipType = role,
            OwnershipStartDate = DateTime.Now,
            AccountStatus = AccountStatus.Active
        };
        context.CustomerAccounts.Add(link);

        context.SaveChanges();

        Output.PrintSuccess("Account '" + accountNumber + "' created and linked to customer " + customerId + " as " + role + " owner.");
    }

    public static void UpdateAccountStatus(AppDbContext context)
    {
        Console.WriteLine("--- Update Account Status ---");

        string accountNumber = Input.ReadText("Account Number : ", 20);
        int customerId = Input.ReadInt("Customer Id    : ");

        CustomerAccount? link = context.CustomerAccounts
            .FirstOrDefault(ca => ca.AccountNumber == accountNumber && ca.CustomerId == customerId);

        if (link == null)
        {
            Output.PrintError("This customer is not linked to this account.");
            return;
        }

        Console.WriteLine("New Status:");
        Console.WriteLine("    1) Active");
        Console.WriteLine("    2) Closed");
        int statusChoice = Input.ReadChoice("  Choice: ", 2);

        link.AccountStatus = statusChoice == 1 ? AccountStatus.Active : AccountStatus.Closed;
        context.SaveChanges();

        Output.PrintSuccess("Status updated to " + link.AccountStatus + ".");
    }

    public static void RemoveAccount(AppDbContext context)
    {
        Console.WriteLine("--- Remove Account From Customer ---");

        string accountNumber = Input.ReadText("Account Number : ", 20);
        int customerId = Input.ReadInt("Customer Id    : ");
        Console.WriteLine();

        CustomerAccount? link = context.CustomerAccounts
            .Include(ca => ca.Account)
            .FirstOrDefault(ca => ca.AccountNumber == accountNumber && ca.CustomerId == customerId);

        if (link == null)
        {
            Output.PrintError("This customer is not linked to this account.");
            return;
        }

        bool hasOtherOwners = context.CustomerAccounts
            .Any(ca => ca.AccountNumber == accountNumber && ca.CustomerId != customerId);

        if (hasOtherOwners)
        {
            context.CustomerAccounts.Remove(link);
            context.SaveChanges();
            Output.PrintSuccess("Ownership link deleted.");
            return;
        }

        if (link.Account.CurrentBalance != 0)
        {
            Output.PrintError("Cannot remove the last owner because the account still has a balance.");
            return;
        }

        context.CustomerAccounts.Remove(link);
        context.Accounts.Remove(link.Account);
        context.SaveChanges();

        Output.PrintSuccess("Ownership link deleted.");
        Output.PrintInfo("    That was the last owner - account '" + accountNumber + "' was also removed.");
    }
}
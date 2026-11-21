


internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("National Bank - Management");

        using (AppDbContext context = new AppDbContext())
        {
            Console.WriteLine("Database Context Created Successfully.");
        }

        Console.WriteLine("Program Finished.");
    }
}

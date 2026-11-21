using System.Globalization;

public static class Input
{
    public static string ReadText(string label, int maxLength)
    {
        while (true)
        {
            Console.Write(label);
            string? text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Output.PrintError("This field cannot be empty. Please try again.");
                continue;
            }

            text = text.Trim();

            if (text.Length > maxLength)
            {
                Output.PrintError("Maximum length is " + maxLength + " characters. Please try again.");
                continue;
            }

            return text;
        }
    }

    public static string ReadDigits(string label, int minLength, int maxLength)
    {
        while (true)
        {
            string text = ReadText(label, maxLength);

            if (text.All(char.IsDigit) && text.Length >= minLength)
                return text;

            Output.PrintError("Please enter digits only (at least " + minLength + " digits).");
        }
    }

    public static string ReadEmail(string label)
    {
        while (true)
        {
            string text = ReadText(label, 150);

            if (text.Contains('@') && text.Contains('.') && !text.Contains(' '))
                return text;

            Output.PrintError("Invalid email. Example: name@mail.com");
        }
    }

    public static int ReadInt(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? text = Console.ReadLine();

            if (int.TryParse(text, out int number) && number > 0)
                return number;

            Output.PrintError("Please enter a valid number.");
        }
    }

    public static int ReadChoice(string label, int max)
    {
        while (true)
        {
            Console.Write(label);
            string? text = Console.ReadLine();

            if (int.TryParse(text, out int number) && number >= 1 && number <= max)
                return number;

            Output.PrintError("Please choose a number from 1 to " + max + ".");
        }
    }

    public static DateTime ReadDate(string label)
    {
        while (true)
        {
            Console.Write(label);
            string? text = Console.ReadLine();

            if (!DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                Output.PrintError("Invalid date. Use the format yyyy-MM-dd, for example 2000-01-25.");
                continue;
            }

            if (date > DateTime.Today)
            {
                Output.PrintError("Date of birth cannot be in the future.");
                continue;
            }

            return date;
        }
    }
}
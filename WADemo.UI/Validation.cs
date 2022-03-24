namespace WADemo.UI;

public class Validation
{
    private static void Prompt2Continue()
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("Press any key to continue...");

        Console.ReadLine();
    }

    private static string PromptRequired(string message)
    {
        string res = PromptUser(message);
        while (string.IsNullOrEmpty(res))
        {
            Console.WriteLine("Input required❗");
            res = PromptUser(message);
        }

        return res;
    }

    private static string PromptUser(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? string.Empty;
    }

    // TODO: Look into using generics so we don't have to duplicate the same stuff for decimal and int
    private static decimal PromptUser4Decimal(string message, decimal min = decimal.MinValue,
        decimal max = decimal.MaxValue)
    {
        decimal result;
        while (!decimal.TryParse(PromptUser(message), out result) || result < min || result > max)
        {
            PromptUser($@"Invalid Input, must be between {min} and {max}
Press Enter to Continue");
        }

        return result;
    }

    private static int PromptUser4Int(string message, int min = int.MinValue, int max = int.MaxValue)
    {
        int result;
        while (!(int.TryParse(PromptUser(message), out result)) || result < min || result > max)
        {
            PromptUser($@"Invalid Input, must be between {min} and {max}
Press Enter to Continue");
        }

        return result;
    }

    // default here means it takes the absolute minimum value for a DateTime
    private static DateTime PromptUser4Date(string message, DateTime max = default)
    {
        DateTime result;
        while (!(DateTime.TryParse(PromptUser(message), out result)) || (max != default && result > max))
        {
            PromptUser($@"Invalid Input, must be before {max}
Press Enter to Continue");
        }

        return result;
    }
}
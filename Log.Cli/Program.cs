// See https://aka.ms/new-console-template for more information

using Log.Application;

class Program
{
    public static void Main(string[] args)
    {
        var filePath = "programming-task-example-data.log";
        if (args.Any())
            filePath = args[0];

        ILogService service = new LogService();

        try
        {
            using (var stream = File.OpenRead(filePath))
            {
                service.ProcessLogFile(stream);

                PrintColored("Number of log entries: ", ConsoleColor.DarkGreen);
                PrintColoredLine($"{service.NumberOfEntries()}\n", ConsoleColor.White);
                
                
                PrintColored("Number of unique IP addresses: ", ConsoleColor.DarkGreen);
                PrintColoredLine($"{service.NumberOfUniqueVisitors()}\n", ConsoleColor.White);

                var topUrls = service.TopVisitedUrls();
                PrintColoredLine("Top Visited URLs: ", ConsoleColor.DarkGreen);
                foreach (var topUrl in topUrls)
                {
                    PrintColoredLine(topUrl, ConsoleColor.White);
                }

                var topVisitors = service.TopActiveVisitors();
                PrintColoredLine("\nTop Visitors: ", ConsoleColor.DarkGreen);
                foreach (var topVisitor in topVisitors)
                {
                    PrintColoredLine(topVisitor, ConsoleColor.White);
                }
            }
        }
        catch (Exception ex)
        {
            PrintColored("Failure: ", ConsoleColor.DarkRed);
            PrintColoredLine(ex.Message, ConsoleColor.White);
        }
    }

    private static void PrintColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
    }

    private static void PrintColoredLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
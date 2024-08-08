using System.Globalization;

namespace Log.Application;

using System.Text.RegularExpressions;

public class LogParser
{
    public static LogEntry Parse(string logLine, bool useRegEx = false)
    {
        return useRegEx ? ParseUsingRegularExpression(logLine) : ParseUsingPartSplitting(logLine);
    }

    private static LogEntry ParseUsingRegularExpression(string logLine)
    {
        string pattern =
            @"(?<ip>\S+) - (?<user>\S+) \[(?<timestamp>[^\]]+)\] ""(?<method>\S+) (?<uri>\S+) (?<protocol>[^\""]+)"" (?<status>\d+) (?<size>\d+) ""(?<referrer>[^\""]*)"" ""(?<agent>[^\""]*)""";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(logLine);

        if (!match.Success)
            throw new FormatException("Invalid log entry format");

        return new LogEntry
        {
            IPAddress = match.Groups["ip"].Value,
            User = match.Groups["user"].Value,
            Timestamp = DateTime.ParseExact(match.Groups["timestamp"].Value, "dd/MMM/yyyy:HH:mm:ss zzz",
                CultureInfo.InvariantCulture),
            RequestMethod = match.Groups["method"].Value,
            RequestUri = match.Groups["uri"].Value,
            Protocol = match.Groups["protocol"].Value,
            StatusCode = int.Parse(match.Groups["status"].Value),
            ResponseSize = int.Parse(match.Groups["size"].Value),
            Referrer = match.Groups["referrer"].Value,
            UserAgent = match.Groups["agent"].Value
        };
    }

    public static LogEntry ParseUsingPartSplitting(string logLine)
    {
        List<string> parts = SplitLogLine(logLine);

        var endpoint = parts[4].Trim('"').Split(" ");

        return new LogEntry
        {
            IPAddress = parts[0],
            User = parts[2],
            Timestamp = DateTime.ParseExact(parts[3].Trim('[', ']'), "dd/MMM/yyyy:HH:mm:ss zzz",
                CultureInfo.InvariantCulture),
            RequestMethod = endpoint[0],
            RequestUri = endpoint[1],
            Protocol = endpoint[2],
            StatusCode = int.Parse(parts[5]),
            ResponseSize = int.Parse(parts[6]),
            Referrer = parts[7].Trim('"'),
            UserAgent = parts[8].Trim('"')
        };
    }

    private static List<string> SplitLogLine(string logLine)
    {
        var parts = new List<string>();
        bool inQuotes = false;
        bool inBrackets = false;
        var currentPart = new List<char>();

        foreach (var ch in logLine)
        {
            if (ch == '"' && !inBrackets)
            {
                inQuotes = !inQuotes;
                currentPart.Add(ch);
            }
            else if (ch == '[')
            {
                inBrackets = true;
                currentPart.Add(ch);
            }
            else if (ch == ']')
            {
                inBrackets = false;
                currentPart.Add(ch);
            }
            else if (ch == ' ' && !inQuotes && !inBrackets)
            {
                if (currentPart.Count > 0)
                {
                    parts.Add(new string(currentPart.ToArray()));
                    currentPart.Clear();
                }
            }
            else
            {
                currentPart.Add(ch);
            }
        }

        if (currentPart.Count > 0)
        {
            parts.Add(new string(currentPart.ToArray()));
        }

        return parts;
    }
}
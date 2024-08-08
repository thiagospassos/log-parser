using System.Reflection.Metadata;

namespace Log.Application;

public interface ILogService
{
    IList<LogEntry> ProcessLogFile(Stream fileStream);

    // • The top 3 most visited URLs
    // • The top 3 most active IP addresses
    int NumberOfEntries();
    IList<string> TopVisitedUrls(int top = 3);
    IList<string> TopActiveVisitors(int top = 3);
    int NumberOfUniqueVisitors();
}

public class LogService : ILogService
{
    private IList<LogEntry> _entries;

    public LogService()
    {
        _entries = new List<LogEntry>();
    }

    public IList<LogEntry> ProcessLogFile(Stream fileStream)
    {
        _entries = new List<LogEntry>();
        var sr = new StreamReader(fileStream);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;
            _entries.Add(LogParser.Parse(line));
        }

        return _entries;
    }

    public int NumberOfEntries()
    {
        return _entries.Count;
    }

    public IList<string> TopVisitedUrls(int top = 3)
    {
        return _entries
            .GroupBy(a => a.RequestUri)
            .OrderByDescending(a => a.Count())
            //in case there are multiple URLs with the same number of visits, the last visited URL will appear first
            .ThenByDescending(a => a.Max(b => b.Timestamp))
            .Take(top)
            .Select(a => a.Key)
            .ToList();
    }

    public IList<string> TopActiveVisitors(int top = 3)
    {
        return _entries
            .GroupBy(a => a.IPAddress)
            .OrderByDescending(a => a.Count())
            //in case there are multiple IP Addresses with the same number of visits, the last visited URL will appear first
            .ThenByDescending(a => a.Max(b => b.Timestamp))
            .Take(top)
            .Select(a => a.Key)
            .ToList();
    }

    public int NumberOfUniqueVisitors()
    {
        return _entries.Select(a => a.IPAddress).Distinct().Count();
    }
}
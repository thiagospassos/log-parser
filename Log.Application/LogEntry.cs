namespace Log.Application;



public class LogEntry
{
    public string IPAddress { get; set; }
    public string User { get; set; }
    public DateTime Timestamp { get; set; }
    public string RequestMethod { get; set; }
    public string RequestUri { get; set; }
    public string Protocol { get; set; }
    public int StatusCode { get; set; }
    public int ResponseSize { get; set; }
    public string Referrer { get; set; }
    public string UserAgent { get; set; }

    public override string ToString()
    {
        return $"IP Address: {IPAddress}\nUser: {User}\nTimestamp: {Timestamp}\nRequest Method: {RequestMethod}\nRequest URI: {RequestUri}\nProtocol: {Protocol}\nStatus Code: {StatusCode}\nResponse Size: {ResponseSize}\nReferrer: {Referrer}\nUser Agent: {UserAgent}";
    }
}
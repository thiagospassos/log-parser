using Log.Application;

namespace Log.Tests;

public class LogTests
{
    private readonly ILogService _logService;
    private const string SampleLogFile = "programming-task-example-data.log";

    public LogTests()
    {
        _logService = new LogService();
        IngestSampleData();
    }

    private void IngestSampleData()
    {
        var stream = File.OpenRead(SampleLogFile);
        _logService.ProcessLogFile(stream);
    }

    [Fact]
    public void ShouldHave23Entries()
    {
        Assert.Equal(23, _logService.NumberOfEntries());
    }

    [Fact]
    public void ShouldHave11UniqueVisitors()
    {
        Assert.Equal(11, _logService.NumberOfUniqueVisitors());
    }

    [Fact]
    public void TopVisitedUrlShouldBeManageWebsites()
    {
        var urls = _logService.TopVisitedUrls();
        Assert.Equal("/docs/manage-websites/", urls.First());
    }

    [Fact]
    public void TopActiveIpAddressShouldBe168_41_191_40()
    {
        var ipAddresses = _logService.TopActiveVisitors(3);
        Assert.Equal("168.41.191.40", ipAddresses.First());
    }
}
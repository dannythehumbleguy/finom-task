namespace ReportService.BusinessLogic.Configuration;

public class CacheOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    
    public TimeSpan TimeToLive { get; set; }
}

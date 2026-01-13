namespace ReportService.BusinessLogic.Configuration;

/// <summary>
/// Настройки кэша.
/// </summary>
public class CacheOptions
{
    /// <summary>
    /// Строка подключения к Redis.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Время жизни записей в кэше.
    /// </summary>
    public TimeSpan TimeToLive { get; set; }
}

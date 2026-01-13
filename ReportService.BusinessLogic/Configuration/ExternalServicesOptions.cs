namespace ReportService.BusinessLogic.Configuration;

/// <summary>
/// Настройки внешних сервисов.
/// </summary>
public class ExternalServicesOptions
{
    /// <summary>
    /// Базовый URL сервиса бухгалтерии.
    /// </summary>
    public string AccountingServiceUrl { get; set; } = string.Empty;

    /// <summary>
    /// Базовый URL сервиса зарплат.
    /// </summary>
    public string SalaryServiceUrl { get; set; } = string.Empty;
}

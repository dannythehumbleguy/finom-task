namespace ReportService.BusinessLogic.Abstractions;

public interface IAccountingServiceClient
{
    Task<string> GetBuhCode(string inn);
}
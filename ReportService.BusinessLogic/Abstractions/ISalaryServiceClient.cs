namespace ReportService.BusinessLogic.Abstractions;

public interface ISalaryServiceClient
{ 
    Task<decimal> GetSalary(string inn, string buhCode);
}
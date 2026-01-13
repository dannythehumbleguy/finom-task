using ReportService.BusinessLogic.Abstractions;

namespace ReportService.Clients;

public class AccountingServiceClient(HttpClient http) : IAccountingServiceClient
{
    public async Task<string> GetBuhCode(string inn) => 
        await http.GetStringAsync("inn/" + inn);
}
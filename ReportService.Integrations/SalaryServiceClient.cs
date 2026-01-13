using System.Net.Http.Json;
using ReportService.BusinessLogic.Abstractions;

namespace ReportService.Clients;

public class SalaryServiceClient(HttpClient http) : ISalaryServiceClient
{
    public async Task<decimal> GetSalary(string inn, string buhCode)
    {
        var result = await http.PostAsJsonAsync("empcode/"+ inn, new { buhCode = buhCode });
        result.EnsureSuccessStatusCode();
        
        return await result.Content.ReadFromJsonAsync<decimal>();
    }
}
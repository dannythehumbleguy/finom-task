using Microsoft.Extensions.Options;
using ReportService.BusinessLogic.Abstractions;
using ReportService.BusinessLogic.Configuration;
using ReportService.BusinessLogic.Entities;

namespace ReportService.BusinessLogic.Services;

public interface IDepartmentService
{
    Task<List<Department>> GetDepartments();
}

public class DepartmentService(IDepartmentRepository departmentRepository, 
    IAccountingServiceClient accountingServiceClient, 
    ISalaryServiceClient salaryServiceClient,
    ICache cache,
    IOptions<CacheOptions> cacheOptions) : IDepartmentService
{
    
    public async Task<List<Department>> GetDepartments()
    {
        var departments = await departmentRepository.GetDepartments();
        foreach (var department in departments)
        {
            foreach (var employee in department.Employees)
            {
                var buhCode = await GetBuhCode(employee.Inn);
                employee.BuhCode = buhCode;
                
                var salary = await salaryServiceClient.GetSalary(employee.Inn, buhCode);
                employee.Salary = salary;
            }
        }
        
        return  departments;
    }

    private async Task<string> GetBuhCode(string inn)
    {
        var buhCodeCacheKey = $"buhcode:{inn}";
        var buhCode = await cache.GetAsync<string>(buhCodeCacheKey);
        if (string.IsNullOrEmpty(buhCode))
        {
            buhCode = await accountingServiceClient.GetBuhCode(inn);
            await cache.SetAsync(buhCodeCacheKey, buhCode, cacheOptions.Value.TimeToLive);
        }

        return buhCode;
    }
}
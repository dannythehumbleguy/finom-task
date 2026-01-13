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
                var buhCodeCacheKey = $"buhcode:{employee.Inn}";
                var buhCode = await cache.GetAsync<string>(buhCodeCacheKey);
        
                if (string.IsNullOrEmpty(buhCode))
                {
                    buhCode = await accountingServiceClient.GetBuhCode(employee.Inn);
                    await cache.SetAsync(buhCodeCacheKey, buhCode, cacheOptions.Value.TimeToLive);
                }
                
                employee.BuhCode = buhCode;
                
                var salary = await salaryServiceClient.GetSalary(employee.Inn, buhCode);
                employee.Salary = salary;
            }
        }
        
        return  departments;
    }
}
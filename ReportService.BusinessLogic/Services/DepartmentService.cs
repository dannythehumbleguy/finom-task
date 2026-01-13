using ReportService.BusinessLogic.Abstractions;
using ReportService.BusinessLogic.Entities;

namespace ReportService.BusinessLogic.Services;

public interface IDepartmentService
{
    Task<List<Department>> GetDepartments();
}

public class DepartmentService(IDepartmentRepository departmentRepository, 
    IAccountingServiceClient accountingServiceClient, 
    ISalaryServiceClient salaryServiceClient) : IDepartmentService
{
    public async Task<List<Department>> GetDepartments()
    {
        var departments = await departmentRepository.GetDepartments();
        foreach (var department in departments)
        {
            foreach (var employee in department.Employees)
            {
                //todo: add cache
                var buhCode = await accountingServiceClient.GetBuhCode(employee.Inn);
                employee.BuhCode = buhCode;
                
                var salary = await salaryServiceClient.GetSalary(employee.Inn, buhCode);
                employee.Salary = salary;
            }
        }
        
        return  departments;
    }
}
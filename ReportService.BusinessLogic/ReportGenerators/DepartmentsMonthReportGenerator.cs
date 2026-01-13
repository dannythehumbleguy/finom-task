using System.Globalization;
using Microsoft.Extensions.Logging;
using ReportService.BusinessLogic.Entities;

namespace ReportService.BusinessLogic.ReportGenerators;

public interface IDepartmentsMonthReportGenerator
{
    Stream Generate(DateOnly date, List<Department> departments);
}

public class DepartmentsMonthReportGenerator(ILogger<DepartmentsMonthReportGenerator> logger) : IDepartmentsMonthReportGenerator
{
    public Stream Generate(DateOnly date, List<Department> departments)
    {
        var reportTitle = date.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
        var report = new Report(reportTitle);
        
        foreach (var department in departments)
        {
            report.HorizonalLine();
            report.NewLine();
            
            report.Text(department.Name, Styles.H3);
            report.NewLine();
            
            foreach (var employee in department.Employees)
            {
                report.Text($"{employee.Name} - ");
                report.Money(employee.Salary);
                report.NewLine();
            }  
            report.NewLine();
            report.Text("Total for department:", Styles.Bold);
            report.Space();
            report.Money(department.EmployeeSalary);
            report.NewLine();
            report.NewLine();
        }
        
        report.HorizonalLine();
        report.NewLine();
        report.Text("Total for the company:", Styles.Bold);
        report.Space();
        var total = departments.Sum(u => u.EmployeeSalary);
        report.Money(total);
        
        var stream = report.Save();
        logger.LogInformation("Report generation completed successfully. Total departments: {DepartmentCount}, Total salary: {TotalSalary}", departments.Count, total);
        
        return stream;
    }
}
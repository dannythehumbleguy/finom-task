using System.Globalization;
using ReportService.BusinessLogic.Entities;

namespace ReportService.BusinessLogic.ReportGenerators;

public interface IDepartmentsMonthReportGenerator
{
    Stream Generate(DateOnly date, List<Department> departments);
}

public class DepartmentsMonthReportGenerator : IDepartmentsMonthReportGenerator
{
    public Stream Generate(DateOnly date, List<Department> departments)
    {
        var report = new Report(date.ToString("MMMM yyyy", CultureInfo.CurrentCulture));
        
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
            report.Text("Total for department: ", Styles.Bold);
            report.Money(department.EmployeeSalary);
            report.NewLine();
            report.NewLine();
        }
        
        report.HorizonalLine();
        report.NewLine();
        report.Text("Total for the company: ", Styles.Bold);
        var total = departments.Sum(u => u.EmployeeSalary);
        report.Money(total);
        
        return report.Save();
    }
}
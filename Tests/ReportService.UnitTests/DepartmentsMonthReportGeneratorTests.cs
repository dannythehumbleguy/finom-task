using System.Globalization;
using Microsoft.Extensions.Logging.Abstractions;
using ReportService.BusinessLogic.Entities;
using ReportService.BusinessLogic.ReportGenerators;

namespace ReportService.UnitTests;

public class DepartmentsMonthReportGeneratorTests
{
    private readonly DepartmentsMonthReportGenerator _generator;

    public DepartmentsMonthReportGeneratorTests()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-IE");
        var logger = NullLogger<DepartmentsMonthReportGenerator>.Instance;
        _generator = new DepartmentsMonthReportGenerator(logger);
    }

    [Fact]
    public void Generate_WithEmptyDepartments_ReturnsReportWithTotals()
    {
        var date = new DateOnly(2024, 1, 1);
        
        var stream = _generator.Generate(date, []);
        
        Assert.NotNull(stream);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        Assert.Contains("January 2024", content);
        Assert.Contains("Total for the company", content);
    }

    [Fact]
    public void Generate_WithDepartments_IncludesDepartmentNames()
    {
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "IT", Employees = [new Employee { Name = "John", Salary = 100000 }, new Employee { Name = "Gary", Salary = 40000 }] },
            new() { Id = 2, Name = "Accounting", Employees = [new Employee { Name = "Mary", Salary = 80000 }, new Employee { Name = "Ben", Salary = 30000 }] }
        };
        
        var stream = _generator.Generate(new DateOnly(2024, 3, 1), departments);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        
        Assert.Contains("IT", content);
        Assert.Contains("Accounting", content);
    }

    [Fact]
    public void Generate_WithDepartments_IncludesEmployeeNames()
    {
        var departments = new List<Department>
        {
            new()
            {
                Id = 1,
                Name = "IT",
                Employees =
                [
                    new Employee { Name = "Andrew Peterson", Salary = 90000 },
                    new Employee { Name = "Elena Smith", Salary = 85000 }
                ]
            }
        };
        
        var stream = _generator.Generate(new DateOnly(2024, 1, 1), departments);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        
        Assert.Contains("Andrew Peterson", content);
        Assert.Contains("Elena Smith", content);
    }

    [Fact]
    public void Generate_WithDepartments_IncludesDepartmentTotals()
    {
        var departments = new List<Department>
        {
            new()
            {
                Id = 1,
                Name = "IT",
                Employees =
                [
                    new Employee { Name = "Dev1", Salary = 50000 },
                    new Employee { Name = "Dev2", Salary = 50000 }
                ]
            }
        };
        
        var stream = _generator.Generate(new DateOnly(2024, 1, 1), departments);
        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        
        Assert.Contains("Total for department", content);
    }
}

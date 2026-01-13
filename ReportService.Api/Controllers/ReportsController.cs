using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportService.BusinessLogic.ReportGenerators;
using ReportService.BusinessLogic.Services;

namespace ReportService.Controllers;

/// <summary>
/// Controller for generating employee salary reports.
/// </summary>
[Route("api/[controller]")]
public class ReportsController(IDepartmentService departmentService, 
    IDepartmentsMonthReportGenerator departmentsMonthReportGenerator) : Controller
{
    /// <summary>
    /// Downloads a monthly salary report for all departments.
    /// </summary>
    /// <param name="date">Report date (year and month).</param>
    /// <returns>A text file containing the report.</returns>
    /// <remarks>
    /// Example request: GET /api/reports/2025/12
    /// </remarks>

    [HttpGet]
    [Route("{year}/{month}")]
    public async Task<FileStreamResult> Download(DateOnly date)
    {
        var departments = await departmentService.GetDepartments(); // no batching?
        var report = departmentsMonthReportGenerator.Generate(date, departments);

        return File(report, "application/octet-stream", $"DepartmentsMonthReport_{date:yyy_MM_dd}.txt");
    }
}
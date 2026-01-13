using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportService.BusinessLogic.ReportGenerators;
using ReportService.BusinessLogic.Services;

namespace ReportService.Controllers;

/// <summary>
/// Контроллер для генерации отчётов по зарплатам сотрудников.
/// </summary>
[Route("api/[controller]")]
public class ReportsController(IDepartmentService departmentService, 
    IDepartmentsMonthReportGenerator departmentsMonthReportGenerator) : Controller
{
    /// <summary>
    /// Скачивает месячный отчёт по зарплатам всех департаментов.
    /// </summary>
    /// <param name="date">Дата отчёта (год и месяц).</param>
    /// <returns>Текстовый файл с отчётом.</returns>
    /// <remarks>
    /// Пример запроса: GET /api/reports/2025/12
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
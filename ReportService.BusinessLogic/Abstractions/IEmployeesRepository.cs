using ReportService.BusinessLogic.Entities;

namespace ReportService.BusinessLogic.Abstractions;

public interface IDepartmentRepository
{
    Task<List<Department>> GetDepartments();
}
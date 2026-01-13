using Dapper;
using Microsoft.Extensions.Logging;
using ReportService.BusinessLogic.Abstractions;
using ReportService.BusinessLogic.Entities;
using ReportService.DataAccess.Common;

namespace ReportService.DataAccess;

public class DepartmentRepository(IDbConnectionFactory connectionFactory, ILogger<DepartmentRepository> logger) : IDepartmentRepository
{
    public async Task<List<Department>> GetDepartments()
    {
        using var connection = connectionFactory.Create();
        var sql =
            """
                SELECT d.id, d.name, e.id as employee_id, e.name, e.inn 
                FROM emps e
                JOIN deps d ON e.departmentid = d.id
                WHERE e.active = TRUE
                ORDER BY d.id, e.id
            """;

        var dict = new Dictionary<long, Department>();
    
        await connection.QueryAsync<Department, Employee, Department>(sql,
            (d, e) =>
            {
                if (d is null) return null;
            
                if (!dict.TryGetValue(d.Id, out var department))
                {
                    department = d;
                    dict[d.Id] = department;
                }
            
                if (e is not null && e.Id != 0)
                    department.Employees.Add(e);

                return department;
            }, 
            splitOn: "employee_id");

        return dict.Values.ToList();
    }
}
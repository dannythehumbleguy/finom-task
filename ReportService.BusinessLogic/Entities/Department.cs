namespace ReportService.BusinessLogic.Entities;

public class Department
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }
    
    public decimal EmployeeSalary => Employees.Sum(e => e.Salary);
}
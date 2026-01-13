using Moq;
using ReportService.BusinessLogic.Abstractions;
using ReportService.BusinessLogic.Entities;
using ReportService.BusinessLogic.Services;

namespace ReportService.UnitTests;

public class DepartmentServiceTests
{
    private readonly Mock<IDepartmentRepository> _repositoryMock;
    private readonly Mock<IAccountingServiceClient> _accountingClientMock;
    private readonly Mock<ISalaryServiceClient> _salaryClientMock;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        _repositoryMock = new Mock<IDepartmentRepository>();
        _accountingClientMock = new Mock<IAccountingServiceClient>();
        _salaryClientMock = new Mock<ISalaryServiceClient>();
        _service = new DepartmentService(
            _repositoryMock.Object,
            _accountingClientMock.Object,
            _salaryClientMock.Object);
    }

    [Fact]
    public async Task GetDepartments_WithNoDepartments_ReturnsEmptyList()
    {
        _repositoryMock.Setup(x => x.GetDepartments()).ReturnsAsync([]);

        var result = await _service.GetDepartments();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDepartments_SetsEmployeeBuhCode()
    {
        var employee = new Employee { Id = 1, Inn = "555" };
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "Finance", Employees = [employee] }
        };
        _repositoryMock.Setup(x => x.GetDepartments()).ReturnsAsync(departments);
        _accountingClientMock.Setup(x => x.GetBuhCode("555")).ReturnsAsync("CODE123");
        _salaryClientMock.Setup(x => x.GetSalary(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(0m);

        var result = await _service.GetDepartments();

        Assert.Equal("CODE123", result[0].Employees[0].BuhCode);
    }

    [Fact]
    public async Task GetDepartments_SetsEmployeeSalary()
    {
        var employee = new Employee { Id = 1, Inn = "555" };
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "Finance", Employees = [employee] }
        };
        _repositoryMock.Setup(x => x.GetDepartments()).ReturnsAsync(departments);
        _accountingClientMock.Setup(x => x.GetBuhCode("555")).ReturnsAsync("CODE123");
        _salaryClientMock.Setup(x => x.GetSalary("555", "CODE123")).ReturnsAsync(90000m);

        var result = await _service.GetDepartments();

        Assert.Equal(90000m, result[0].Employees[0].Salary);
    }

    [Fact]
    public async Task GetDepartments_WithMultipleEmployees_SetsAllSalaries()
    {
        var departments = new List<Department>
        {
            new()
            {
                Id = 1,
                Name = "IT",
                Employees =
                [
                    new Employee { Id = 1, Inn = "111" },
                    new Employee { Id = 2, Inn = "222" }
                ]
            }
        };
        _repositoryMock.Setup(x => x.GetDepartments()).ReturnsAsync(departments);
        _accountingClientMock.Setup(x => x.GetBuhCode("111")).ReturnsAsync("BUH1");
        _accountingClientMock.Setup(x => x.GetBuhCode("222")).ReturnsAsync("BUH2");
        _salaryClientMock.Setup(x => x.GetSalary("111", "BUH1")).ReturnsAsync(50000m);
        _salaryClientMock.Setup(x => x.GetSalary("222", "BUH2")).ReturnsAsync(60000m);

        var result = await _service.GetDepartments();

        Assert.Equal(50000m, result[0].Employees[0].Salary);
        Assert.Equal(60000m, result[0].Employees[1].Salary);
    }

    [Fact]
    public async Task GetDepartments_WithMultipleDepartments_ReturnsAll()
    {
        var departments = new List<Department>
        {
            new() { Id = 1, Name = "IT", Employees = [new Employee { Inn = "111" }] },
            new() { Id = 2, Name = "HR", Employees = [new Employee { Inn = "222" }] }
        };
        _repositoryMock.Setup(x => x.GetDepartments()).ReturnsAsync(departments);
        _accountingClientMock.Setup(x => x.GetBuhCode(It.IsAny<string>())).ReturnsAsync("BUH");
        _salaryClientMock.Setup(x => x.GetSalary(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(50000m);

        var result = await _service.GetDepartments();

        Assert.Equal(2, result.Count);
        Assert.Equal("IT", result[0].Name);
        Assert.Equal("HR", result[1].Name);
    }
}

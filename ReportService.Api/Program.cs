using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.AspNetCore;
using ReportService.BusinessLogic.Abstractions;
using ReportService.BusinessLogic.ReportGenerators;
using ReportService.BusinessLogic.Services;
using ReportService.Clients;
using ReportService.DataAccess;
using ReportService.DataAccess.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IDbConnectionFactory>(sp => 
    new NpgsqlConnectionFactory(sp.GetRequiredService<IConfiguration>().GetConnectionString("Default")) );

builder.Services.AddScoped<IDepartmentsMonthReportGenerator, DepartmentsMonthReportGenerator>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddHttpClient<IAccountingServiceClient, AccountingServiceClient>(client =>
{ client.BaseAddress = new Uri("http://buh.local/api/"); });
builder.Services.AddHttpClient<ISalaryServiceClient, SalaryServiceClient>(client =>
{ client.BaseAddress = new Uri("http://salary.local/api/"); });



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseCors(); 
app.MapControllers();

app.Run();
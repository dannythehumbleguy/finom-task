using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.AspNetCore;
using ReportService.BusinessLogic.Abstractions;
using ReportService.BusinessLogic.Configuration;
using ReportService.BusinessLogic.ReportGenerators;
using ReportService.BusinessLogic.Services;
using ReportService.DataAccess;
using ReportService.DataAccess.Common;
using ReportService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IDbConnectionFactory>(sp => 
    new NpgsqlConnectionFactory(sp.GetRequiredService<IConfiguration>().GetConnectionString("Default")) );

builder.Services.Configure<CacheOptions>(builder.Configuration.GetSection("CacheSettings"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    var cacheOptions = builder.Configuration.GetSection("CacheSettings").Get<CacheOptions>();
    options.Configuration = cacheOptions?.ConnectionString ?? string.Empty;
});

builder.Services.AddScoped<ICache, RedisCache>();
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
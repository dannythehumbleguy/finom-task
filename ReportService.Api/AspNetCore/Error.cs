using System;

namespace ReportService.AspNetCore;

public class Error(string message)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = message;
}
using System.Data;

namespace ReportService.DataAccess.Common;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
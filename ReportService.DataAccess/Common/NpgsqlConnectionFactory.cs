using System.Data;
using Npgsql;

namespace ReportService.DataAccess.Common;

public sealed class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _cs;

    public NpgsqlConnectionFactory(string cs) => _cs = cs;

    public IDbConnection Create() => new NpgsqlConnection(_cs);
}
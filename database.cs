using Npgsql;

namespace ISM.Core;

public class Database
{
    private readonly string _connectionString;

    public Database(string? connectionString = null)
    {
        _connectionString = connectionString
            ?? Environment.GetEnvironmentVariable("ISM_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Database=ism_db;Username=postgres";
    }

    public NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}

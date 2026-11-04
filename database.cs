using Npgsql;

namespace ISM.Core;

public class Database
{
    // Hardcoded PostgreSQL connection string values
    private readonly string _connectionString = "Host=localhost;Port=5432;Database=ism_db;Username=postgres;Password=your_password";

    public Database()
    {
        // No constructor parameters needed since values are hardcoded, 
        // though you can still keep a parameterless constructor or pass it if preferred.
    }

    public NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}
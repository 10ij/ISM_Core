using Npgsql;

namespace ISM.Core;

public class MasterManager
{
    private readonly Database _database;

    public MasterManager(Database database)
    {
        _database = database;
    }  

    /// <summary>
    /// Creates a new tenant
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Address1"></param>
    /// <param name="Address2"></param>
    /// <param name="Address3"></param>
    /// <param name="Telephone"></param>
    /// <param name="Email"></param>
    /// <param name="Website"></param>
    /// <param name="ReportingPeriodStart"></param>
    /// <param name="ReportingPeriodLength"></param>
    /// <returns></returns>
    public int Create(string Name, string? Address1, string? Address2, string? Address3, string? Telephone, string? Email, string? Website, DateOnly ReportingPeriodStart, int ReportingPeriodLength)
    {
        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("INSERT INTO master (name, address_1, address_2, address_3, telephone, email, website, reporting_period_start, reporting_period_length) VALUES (@Name, @Address1, @Address2, @Address3, @Telephone, @Email, @Website, @ReportingPeriodStart, @ReportingPeriodLength) RETURNING id", conn);
        cmd.Parameters.AddWithValue("Name", Name);
        cmd.Parameters.AddWithValue("Address1", Address1 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Address2", Address2 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Address3", Address3 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Telephone", Telephone ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Email", Email ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Website", Website ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("ReportingPeriodStart", ReportingPeriodStart.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("ReportingPeriodLength", ReportingPeriodLength);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    /// <summary>
    /// Updates an existing tenant
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="Name"></param>
    /// <param name="Address1"></param>
    /// <param name="Address2"></param>
    /// <param name="Address3"></param>
    /// <param name="Telephone"></param>
    /// <param name="Email"></param>
    /// <param name="Website"></param>
    /// <param name="ReportingPeriodStart"></param>
    /// <param name="ReportingPeriodLength"></param>
    public bool Update(int ID, string Name, string? Address1, string? Address2, string? Address3, string? Telephone, string? Email, string? Website, DateOnly ReportingPeriodStart, int ReportingPeriodLength)
    {
        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("UPDATE master SET name = @Name, address_1 = @Address1, address_2 = @Address2, address_3 = @Address3, telephone = @Telephone, email = @Email, website = @Website, reporting_period_start = @ReportingPeriodStart, reporting_period_length = @ReportingPeriodLength WHERE id = @ID", conn);
        cmd.Parameters.AddWithValue("ID", ID);
        cmd.Parameters.AddWithValue("Name", Name);
        cmd.Parameters.AddWithValue("Address1", Address1 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Address2", Address2 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Address3", Address3 ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Telephone", Telephone ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Email", Email ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("Website", Website ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("ReportingPeriodStart", ReportingPeriodStart.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("ReportingPeriodLength", ReportingPeriodLength);

        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Deletes a tenant by ID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public bool Delete(int ID)
    {
        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("DELETE FROM master WHERE id = @ID", conn);
        cmd.Parameters.AddWithValue("ID", ID);

        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Gives all tenants including archived ones
    /// </summary>
    /// <returns></returns>
    public List<Master> ListAll()
    {
        var tenants = new List<Master>();

        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("SELECT id, name, is_archived, date_created, date_modified FROM master", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            tenants.Add(new Master
            {
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                IsArchived = reader.GetBoolean(reader.GetOrdinal("is_archived")),
                DateCreated = reader.GetDateTime(reader.GetOrdinal("date_created")),
                DateModified = reader.GetDateTime(reader.GetOrdinal("date_modified"))
            });
        }

        return tenants;
    }

    public bool Exists(int masterId)
    {
        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM master WHERE id = @ID", conn);
        cmd.Parameters.AddWithValue("ID", masterId);

        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }
    
    public bool IsArchived(int masterId)
    {
        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("SELECT is_archived FROM master WHERE id = @ID", conn);
        cmd.Parameters.AddWithValue("ID", masterId);

        return Convert.ToBoolean(cmd.ExecuteScalar());
    }

    public bool Archive(int masterId)
    {
        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("UPDATE master SET is_archived = true WHERE id = @ID", conn);
        cmd.Parameters.AddWithValue("ID", masterId);

        return cmd.ExecuteNonQuery() > 0;
    }

    public bool Unarchive(int masterId)
    {
        using var conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand("UPDATE master SET is_archived = false WHERE id = @ID", conn);
        cmd.Parameters.AddWithValue("ID", masterId);

        return cmd.ExecuteNonQuery() > 0;
    }
}
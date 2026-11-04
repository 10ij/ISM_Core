using Npgsql;

namespace ISM.Core;

public class ParentManager
{
    private readonly Database _database;

    public ParentManager(Database database)
    {
        _database = database;
    }

    /// <summary>
    /// Creates a new parent record in the database and returns the ID of the newly created parent, AI
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="title"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="gender"></param>
    /// <param name="dob"></param>
    /// <param name="homeTelephone"></param>
    /// <param name="mobileTelephone"></param>
    /// <param name="address1"></param>
    /// <param name="notes"></param>
    /// <returns></returns>
    public int Create(
        int tenantId,
        PersonTitle? title,
        string firstName,
        string lastName,
        Gender? gender,
        DateOnly? dob,
        string? homeTelephone,
        string? mobileTelephone,
        string? address1,
        string? notes
    )
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            INSERT INTO parent (tenant_id, title, first_name, last_name, gender, date_of_birth, home_telephone, mobile_telephone, address_1, notes)
            VALUES (@TenantID, @Title, @FirstName, @LastName, @Gender, @DateOfBirth, @HomeTelephone, @MobileTelephone, @Address1, @Notes)
            RETURNING id;";

        using var cmd = new NpgsqlCommand(Query, Conn);

        cmd.Parameters.AddWithValue("TenantID", tenantId);
        cmd.Parameters.AddWithValue("Title", (object?)title ?? DBNull.Value);
        cmd.Parameters.AddWithValue("FirstName", firstName);
        cmd.Parameters.AddWithValue("LastName", lastName);
        cmd.Parameters.AddWithValue("Gender", (object?)gender ?? DBNull.Value);
        cmd.Parameters.AddWithValue("DateOfBirth", (object?)dob ?? DBNull.Value);
        cmd.Parameters.AddWithValue("HomeTelephone", (object?)homeTelephone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("MobileTelephone", (object?)mobileTelephone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Address1", (object?)address1 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Notes", (object?)notes ?? DBNull.Value);

        return (int)cmd.ExecuteScalar();
    } 

    /// <summary>
    /// Updates existing parent
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="tenantId"></param>
    /// <param name="title"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="gender"></param>
    /// <param name="dob"></param>
    /// <param name="homeTelephone"></param>
    /// <param name="UpdateHomeTelephone"></param>
    /// <param name="mobileTelephone"></param>
    /// <param name="UpdateMobileTelephone"></param>
    /// <param name="UpdateAddress1"></param>
    /// <param name="UpdateNotes"></param>
    /// <param name="address1"></param>
    /// <param name="notes"></param>
    /// <returns></returns>
    public bool Update(
        int parentId,
        int tenantId,
        PersonTitle? title,
        string? firstName,
        string? lastName,
        Gender? gender,
        DateOnly? dob,
        string? homeTelephone,
        bool UpdateHomeTelephone,
        string? mobileTelephone,
        bool UpdateMobileTelephone,
        bool UpdateAddress1,
        bool UpdateNotes,
        string? address1,
        string? notes
    )
    {
        using var Conn = _database.GetConnection();
        // Because the only time a gender isnt set is when a migration happens
        // And every new student will have a gender set
        // Its fine with coalesce
        var Query = @"
            UPDATE parent
            SET 
                title = COALESCE(@Title, title),
                first_name = COALESCE(@FirstName, first_name),
                last_name = COALESCE(@LastName, last_name),
                gender = COALESCE(@Gender, gender),
                date_of_birth = COALESCE(@DateOfBirth, date_of_birth),
                hometelephone = CASE WHEN @UpdateHomeTelephone THEN @HomeTelephone ELSE hometelephone END,
                mobiletelephone = CASE WHEN @UpdateMobileTelephone THEN @MobileTelephone ELSE mobiletelephone END,
                address_1 = CASE WHEN @UpdateAddress1 THEN @Address1 ELSE address_1 END,
                notes = CASE WHEN @UpdateNotes THEN @Notes ELSE notes END,
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);

        // Not nullable

        cmd.Parameters.AddWithValue("Id", parentId);
        cmd.Parameters.AddWithValue("FirstName", (object?)firstName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("LastName", (object?)lastName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        // Shouldnt need to null anyways

        cmd.Parameters.AddWithValue("Title", (object?)title ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Gender", (object?)gender ?? DBNull.Value);
        cmd.Parameters.AddWithValue("DateOfBirth", (object?)dob ?? DBNull.Value);

        // Might need to null

        cmd.Parameters.AddWithValue("Notes", (object?)notes ?? DBNull.Value);
        if (UpdateNotes)
        {
            cmd.Parameters.AddWithValue("UpdateNotes", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateNotes", false);
        }

        cmd.Parameters.AddWithValue("HomeTelephone", (object?)homeTelephone ?? DBNull.Value);
        if (UpdateHomeTelephone)
        {
            cmd.Parameters.AddWithValue("UpdateHomeTelephone", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateHomeTelephone", false);
        }

        cmd.Parameters.AddWithValue("MobileTelephone", (object?)mobileTelephone ?? DBNull.Value);
        if (UpdateMobileTelephone)
        {
            cmd.Parameters.AddWithValue("UpdateMobileTelephone", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateMobileTelephone", false);
        }

        cmd.Parameters.AddWithValue("Address1", (object?)address1 ?? DBNull.Value);
        if (UpdateAddress1)
        {
            cmd.Parameters.AddWithValue("UpdateAddress1", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateAddress1", false);
        }


        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Deletes a parent, should rarely be used
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public bool Delete(int parentId, int tenantId)
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            DELETE FROM parent
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", parentId);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Returns a parent record by ID and tenant ID
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public Parent Read(
        int parentId,
        int tenantId
    )
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            SELECT *
            FROM parent
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", parentId);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        using var reader = cmd.ExecuteReader();
        return new Parent
        {
            ID = reader.GetInt32(reader.GetOrdinal("id")),
            TenantID = reader.GetInt32(reader.GetOrdinal("tenant_id")),
            Title = reader.IsDBNull(reader.GetOrdinal("title")) ? null : (PersonTitle?)reader.GetInt32(reader.GetOrdinal("title")),
            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
            LastName = reader.GetString(reader.GetOrdinal("last_name")),
            Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? null : (Gender?)reader.GetInt32(reader.GetOrdinal("gender")),
            DoB = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("date_of_birth")),
            HomeTelephone = reader.IsDBNull(reader.GetOrdinal("home_telephone")) ? null : reader.GetString(reader.GetOrdinal("home_telephone")),
            MobileTelephone = reader.IsDBNull(reader.GetOrdinal("mobile_telephone")) ? null : reader.GetString(reader.GetOrdinal("mobile_telephone")),
            Address1 = reader.IsDBNull(reader.GetOrdinal("address_1")) ? null : reader.GetString(reader.GetOrdinal("address_1")),
            Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
            Archived = reader.GetBoolean(reader.GetOrdinal("archived")),
            DateModified = reader.GetDateTime(reader.GetOrdinal("date_modified")),
            DateCreated = reader.GetDateTime(reader.GetOrdinal("date_created"))
        };
    }

    /// <summary>
    /// Archives a parent record 
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public bool Archive(int parentId, int tenantId)
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            UPDATE parent
            SET archived = TRUE
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", parentId);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        return (bool)cmd.ExecuteScalar();
    }

    /// <summary>
    /// Unarchives a parent record
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public bool Unarchive(int parentId, int tenantId)
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            UPDATE parent
            SET archived = FALSE
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", parentId);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        return (bool)cmd.ExecuteScalar();
    }
    
    /// <summary>
    /// Lists all parents, even archived, returns a list with parent object
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public List<Parent> ListAll(
        int tenantId
    )
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            SELECT *
            FROM parent
            WHERE tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        using var reader = cmd.ExecuteReader();
        var parents = new List<Parent>();
        while (reader.Read())
        {
            parents.Add(new Parent
            {
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                TenantID = reader.GetInt32(reader.GetOrdinal("tenant_id")),
                Title = reader.IsDBNull(reader.GetOrdinal("title")) ? null : (PersonTitle?)reader.GetInt32(reader.GetOrdinal("title")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? null : (Gender?)reader.GetInt32(reader.GetOrdinal("gender")),
                DoB = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("date_of_birth")),
                HomeTelephone = reader.IsDBNull(reader.GetOrdinal("home_telephone")) ? null : reader.GetString(reader.GetOrdinal("home_telephone")),
                MobileTelephone = reader.IsDBNull(reader.GetOrdinal("mobile_telephone")) ? null : reader.GetString(reader.GetOrdinal("mobile_telephone")),
                Address1 = reader.IsDBNull(reader.GetOrdinal("address_1")) ? null : reader.GetString(reader.GetOrdinal("address_1")),
                Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                Archived = reader.GetBoolean(reader.GetOrdinal("archived")),
                DateModified = reader.GetDateTime(reader.GetOrdinal("date_modified")),
                DateCreated = reader.GetDateTime(reader.GetOrdinal("date_created"))
            });
        }
        return parents;
    }

    /// <summary>
    /// Returns the count of parents for a given tenant
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public int Count(
        int tenantId
    )
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            SELECT COUNT(*)
            FROM parent
            WHERE tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        return (int)cmd.ExecuteScalar();
    }

    /// <summary>
    /// Searches for parents by first or last name, returns a list of parent objects
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
    public List<Parent> Search(
        int tenantId,
        string searchTerm
    )
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            SELECT *
            FROM parent
            WHERE tenant_id = @TenantID AND (first_name ILIKE @SearchTerm OR last_name ILIKE @SearchTerm);";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("TenantID", tenantId);
        cmd.Parameters.AddWithValue("SearchTerm", $"%{searchTerm}%");

        using var reader = cmd.ExecuteReader();
        var parents = new List<Parent>();
        while (reader.Read())
        {
            parents.Add(new Parent
            {
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                TenantID = reader.GetInt32(reader.GetOrdinal("tenant_id")),
                Title = reader.IsDBNull(reader.GetOrdinal("title")) ? null : (PersonTitle?)reader.GetInt32(reader.GetOrdinal("title")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? null : (Gender?)reader.GetInt32(reader.GetOrdinal("gender")),
                DoB = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("date_of_birth")),
                HomeTelephone = reader.IsDBNull(reader.GetOrdinal("home_telephone")) ? null : reader.GetString(reader.GetOrdinal("home_telephone")),
                MobileTelephone = reader.IsDBNull(reader.GetOrdinal("mobile_telephone")) ? null : reader.GetString(reader.GetOrdinal("mobile_telephone")),
                Address1 = reader.IsDBNull(reader.GetOrdinal("address_1")) ? null : reader.GetString(reader.GetOrdinal("address_1")),
                Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                Archived = reader.GetBoolean(reader.GetOrdinal("archived")),
                DateModified = reader.GetDateTime(reader.GetOrdinal("date_modified")),
                DateCreated = reader.GetDateTime(reader.GetOrdinal("date_created"))
            });
        }
        return parents;
    }
    
    /// <summary>
    /// Checks if a parent is archived
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public bool IsArchived(int parentId, int tenantId)
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            SELECT archived
            FROM parent
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", parentId);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        return (bool)cmd.ExecuteScalar();
    }  

    /// <summary>
    /// Checks if a parent exists by ID 
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public bool Exists(int parentId, int tenantId)
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            SELECT EXISTS(
                SELECT 1
                FROM parent
                WHERE id = @Id AND tenant_id = @TenantID
            );";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", parentId);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        return (bool)cmd.ExecuteScalar();
    }

    public List<int> FindRelatedStudentIds(int parentId, int tenantId)
    {
        using var Conn = _database.GetConnection();
        var Query = @"
            SELECT student_id
            FROM parent_student
            WHERE parent_id = @ParentID AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("ParentID", parentId);
        cmd.Parameters.AddWithValue("TenantID", tenantId);

        using var reader = cmd.ExecuteReader();
        var studentIds = new List<int>();
        while (reader.Read())
        {
            studentIds.Add(reader.GetInt32(reader.GetOrdinal("student_id")));
        }
        return studentIds;
    }

}
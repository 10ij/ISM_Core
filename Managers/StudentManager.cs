
using Npgsql;

namespace ISM.Core;

public class StudentManager
{
    private readonly Database _database;

    public StudentManager(Database database)
    {
        _database = database;
    }

    /// <summary>
    /// Adds a new student tp the db, VSCode auto summary:
    /// </summary>
    /// <param name="TenantID"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="Title"></param>
    /// <param name="Gender"></param>
    /// <param name="DateOfBirth"></param>
    /// <param name="Notes"></param>
    /// <param name="Allergies"></param>
    /// <param name="MedicalConditions"></param>
    /// <param name="Telephone"></param>
    public int Create(
        int TenantID, 
        string FirstName,
        string LastName, 

        // Optional
        int? Title = null, 
        int? Gender = null, 
        DateOnly? DateOfBirth = null, 
        string? Notes = null, 
        string? Allergies = null, 
        string? MedicalConditions = null, 
        string? Telephone = null)
    {
        using var Conn = _database.GetConnection();
        
        // This is how they do paramaterised queries in Cs
        var Query = @"
            INSERT INTO student (
                tenant_id, 
                title, 
                first_name, 
                last_name, 
                gender, 
                date_of_birth, 
                notes, 
                allergies, 
                medical_conditions, 
                telephone
            ) 
            VALUES (
                @TenantID, 
                @Title, 
                @FirstName, 
                @LastName, 
                @Gender, 
                @DateOfBirth, 
                @Notes, 
                @Allergies, 
                @MedicalConditions, 
                @Telephone
            )
            RETURNING id;";
        
        using var cmd = new NpgsqlCommand(Query, Conn);

        // Directly bind parameters using NpgsqlCommand's AddWithValue
        cmd.Parameters.AddWithValue("TenantID", TenantID);
        cmd.Parameters.AddWithValue("Title", (object?)Title ?? DBNull.Value);
        cmd.Parameters.AddWithValue("FirstName", FirstName);
        cmd.Parameters.AddWithValue("LastName", LastName);
        cmd.Parameters.AddWithValue("Gender", (object?)Gender ?? DBNull.Value);
        
        // Npgsql handles DateOnly? natively; pass DBNull if null
        cmd.Parameters.AddWithValue("DateOfBirth", (object?)DateOfBirth ?? DBNull.Value);
        
        cmd.Parameters.AddWithValue("Notes", (object?)Notes ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Allergies", (object?)Allergies ?? DBNull.Value);
        cmd.Parameters.AddWithValue("MedicalConditions", (object?)MedicalConditions ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Telephone", (object?)Telephone ?? DBNull.Value);

        var newId = Convert.ToInt32(cmd.ExecuteScalar());
        return newId;
    }


    /// <summary>
    /// Updates a student record as a whole, try to use smaller methods instead
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantID"></param>
    /// <param name="Title"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    /// <param name="Gender"></param>
    /// <param name="DateOfBirth"></param>
    /// <param name="Notes"></param>
    /// <param name="Allergies"></param>
    /// <param name="MedicalConditions"></param>
    /// <param name="Telephone"></param>
    /// <returns></returns>
    public bool Update(
        int Id,
        int TenantID,
        int? Title = null,
        string? FirstName = null,
        string? LastName = null,
        int? Gender = null,
        DateOnly? DateOfBirth = null,
        string? Notes = null,
        string? Allergies = null,
        string? MedicalConditions = null,
        string? Telephone = null,
        bool UpdateNotes = false,
        bool UpdateAllergies = false,
        bool UpdateMedicalConditions = false,
        bool UpdateTelephone = false)
    {
        using var Conn = _database.GetConnection();

        var Query = @"
            UPDATE student
            SET 
                title = COALESCE(@Title, title),
                first_name = COALESCE(@FirstName, first_name),
                last_name = COALESCE(@LastName, last_name),
                gender = COALESCE(@Gender, gender),
                date_of_birth = COALESCE(@DateOfBirth, date_of_birth),
                notes = CASE WHEN @UpdateNotes THEN @Notes ELSE notes END,
                allergies = CASE WHEN @UpdateAllergies THEN @Allergies ELSE allergies END,
                medical_conditions = CASE WHEN @UpdateMedicalConditions THEN @MedicalConditions ELSE medical_conditions END,
                telephone = CASE WHEN @UpdateTelephone THEN @Telephone ELSE telephone END
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);

        // Not nullable

        cmd.Parameters.AddWithValue("Id", Id);
        cmd.Parameters.AddWithValue("FirstName", (object?)FirstName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("LastName", (object?)LastName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("TenantID", TenantID);

        // Shouldnt need to null anyways

        cmd.Parameters.AddWithValue("Title", (object?)Title ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Gender", (object?)Gender ?? DBNull.Value);
        cmd.Parameters.AddWithValue("DateOfBirth", (object?)DateOfBirth ?? DBNull.Value);

        // Might need to null

        cmd.Parameters.AddWithValue("Notes", (object?)Notes ?? DBNull.Value);
        if (UpdateNotes)
        {
            cmd.Parameters.AddWithValue("UpdateNotes", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateNotes", false);
        }
        cmd.Parameters.AddWithValue("Allergies", (object?)Allergies ?? DBNull.Value);
        if (UpdateAllergies)
        {
            cmd.Parameters.AddWithValue("UpdateAllergies", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateAllergies", false);
        }
        cmd.Parameters.AddWithValue("MedicalConditions", (object?)MedicalConditions ?? DBNull.Value);
        if (UpdateMedicalConditions)
        {
            cmd.Parameters.AddWithValue("UpdateMedicalConditions", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateMedicalConditions", false);
        }
        cmd.Parameters.AddWithValue("Telephone", (object?)Telephone ?? DBNull.Value);
        if (UpdateTelephone)
        {
            cmd.Parameters.AddWithValue("UpdateTelephone", true);
        }
        else
        {
            cmd.Parameters.AddWithValue("UpdateTelephone", false);
        }

        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Pulls all student data, but not its relationships
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantID"></param>
    /// <returns></returns>
    public Student? Read(
        int Id,
        int TenantID
    )
    {
        using var Conn = _database.GetConnection();

        var Query = @"
            SELECT *
            FROM student
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", Id);
        cmd.Parameters.AddWithValue("TenantID", TenantID);

        using var reader = cmd.ExecuteReader();

        // If a records is found its data is put on a new student object
        if (reader.Read())
        {
            return new Student
            {
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                TenantID = reader.GetInt32(reader.GetOrdinal("tenant_id")),
                Title = reader.IsDBNull(reader.GetOrdinal("title")) ? null : (PersonTitle)reader.GetInt32(reader.GetOrdinal("title")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                Gender = reader.IsDBNull(reader.GetOrdinal("gender")) ? null : (Gender)reader.GetInt32(reader.GetOrdinal("gender")),
                DateOfBirth = reader.IsDBNull(reader.GetOrdinal("date_of_birth")) ? null : reader.GetFieldValue<DateOnly>(reader.GetOrdinal("date_of_birth")),
                Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString(reader.GetOrdinal("notes")),
                Allergies = reader.IsDBNull(reader.GetOrdinal("allergies")) ? null : reader.GetString(reader.GetOrdinal("allergies")),
                MedicalConditions = reader.IsDBNull(reader.GetOrdinal("medical_conditions")) ? null : reader.GetString(reader.GetOrdinal("medical_conditions")),
                Telephone = reader.IsDBNull(reader.GetOrdinal("telephone")) ? null : reader.GetString(reader.GetOrdinal("telephone"))
            };
        }

        return null;
    }

    /// <summary>
    /// Deletes a student record, should never be used unless a grave mistake was entered
    /// Use Archival to remove old students instead
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantID"></param>
    /// <returns></returns>
    public bool Delete(
        int Id,
        int TenantID
    )
    {
        using var Conn = _database.GetConnection();

        var Query = @"
            DELETE FROM student
            WHERE id = @Id AND tenant_id = @TenantID;";

        using var cmd = new NpgsqlCommand(Query, Conn);
        cmd.Parameters.AddWithValue("Id", Id);
        cmd.Parameters.AddWithValue("TenantID", TenantID);

        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Archives a record, check if its alraedy archived before calling
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantID"></param>
    /// <returns></returns>
    public bool Archive(int Id, int TenantID)
    {
        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            UPDATE student
            SET archived_at = now()
            WHERE id = @Id AND tenant_id = @TenantID AND archived_at IS NULL;", Conn);
        cmd.Parameters.AddWithValue("Id", Id);
        cmd.Parameters.AddWithValue("TenantID", TenantID);
        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Unarchives a record, check if its alraedy unarchived before calling
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantID"></param>
    /// <returns></returns>
    public bool Unarchive(int Id, int TenantID)
    {
        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            UPDATE student
            SET archived_at = NULL
            WHERE id = @Id AND tenant_id = @TenantID AND archived_at IS NOT NULL;", Conn);
        cmd.Parameters.AddWithValue("Id", Id);
        cmd.Parameters.AddWithValue("TenantID", TenantID);
        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Returns a list of all students for given tenant, including archived ones
    /// </summary>
    /// <param name="TenantID"></param>
    /// <returns></returns>
    public List<Student> ListAll(int TenantID)
    {
        var students = new List<Student>();

        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT id, first_name, last_name, allergies
            FROM student
            WHERE tenant_id = @TenantID;", Conn);
        cmd.Parameters.AddWithValue("TenantID", TenantID);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                TenantID = reader.GetInt32(reader.GetOrdinal("tenant_id")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                Allergies = reader.IsDBNull(reader.GetOrdinal("allergies")) ? null : reader.GetString(reader.GetOrdinal("allergies"))
            });
        }

        return students;
    }
    
    /// <summary>
    /// Returns the count of students for a given tenant, optionally including archived students
    /// </summary>
    /// <param name="TenantID"></param>
    /// <param name="IncludeArchived"></param>
    /// <returns></returns>
    public int Count(int TenantID)
    {
        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*)
            FROM student
            WHERE tenant_id = @TenantID;", Conn);
        cmd.Parameters.AddWithValue("TenantID", TenantID);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    /// <summary>
    /// Search by Fn or Ln, returns same as listall
    /// </summary>
    /// <param name="TenantID"></param>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
    public List<Student> Search(int TenantID, string searchTerm)
    {
        var students = new List<Student>();

        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT id, first_name, last_name, allergies
            FROM student
            WHERE tenant_id = @TenantID AND (first_name ILIKE @SearchTerm OR last_name ILIKE @SearchTerm);", Conn);
        cmd.Parameters.AddWithValue("TenantID", TenantID);
        cmd.Parameters.AddWithValue("SearchTerm", $"%{searchTerm}%");

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            students.Add(new Student
            {
                ID = reader.GetInt32(reader.GetOrdinal("id")),
                TenantID = reader.GetInt32(reader.GetOrdinal("tenant_id")),
                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                Allergies = reader.IsDBNull(reader.GetOrdinal("allergies")) ? null : reader.GetString(reader.GetOrdinal("allergies"))
            });
        }

        return students;
    }
    
    /// <summary>
    /// Checks if a student record is archived
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantID"></param>
    /// <returns></returns>
    public bool IsArchived(int Id, int TenantID)
    {
        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT archived_at IS NOT NULL
            FROM student
            WHERE id = @Id AND tenant_id = @TenantID;", Conn);
        cmd.Parameters.AddWithValue("Id", Id);
        cmd.Parameters.AddWithValue("TenantID", TenantID);

        var result = cmd.ExecuteScalar();
        return result != null && (bool)result;
    }

    /// <summary>
    /// Does the student exist
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="TenantID"></param>
    /// <returns></returns>
    public bool Exists(int Id, int TenantID)
    {
        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT EXISTS(
                SELECT 1
                FROM student
                WHERE id = @Id AND tenant_id = @TenantID
            );", Conn);
        cmd.Parameters.AddWithValue("Id", Id);
        cmd.Parameters.AddWithValue("TenantID", TenantID);

        return (bool)cmd.ExecuteScalar();
    }
    
    /// <summary>
    /// Returns list of Parents IDs
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public List<int> FindParentIDs(int studentId, int tenantId)
    {
        var parentIds = new List<int>();

        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT parent_id
            FROM parent_student
            WHERE student_id = @StudentId AND tenant_id = @TenantId;", Conn);
        cmd.Parameters.AddWithValue("StudentId", studentId);
        cmd.Parameters.AddWithValue("TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            parentIds.Add(reader.GetInt32(reader.GetOrdinal("parent_id")));
        }

        return parentIds;
    }

    /// <summary>
    /// Returns list of Enrollment IDs
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public List<int> FindEnrollmentIDs(int studentId, int tenantId)
    {
        var enrollmentIds = new List<int>();

        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT id
            FROM enrollment
            WHERE student_id = @StudentId AND tenant_id = @TenantId;", Conn);
        cmd.Parameters.AddWithValue("StudentId", studentId);
        cmd.Parameters.AddWithValue("TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            enrollmentIds.Add(reader.GetInt32(reader.GetOrdinal("id")));
        }

        return enrollmentIds;
    }

    /// <summary>
    /// Returns list of all the financial stuff a child has that is linked to them
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public List<int> FindLedgerEntries (int studentId, int tenantId)
    {
        var ledgerEntryIds = new List<int>();

        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT id
            FROM student_ledger
            WHERE student_id = @StudentId AND tenant_id = @TenantId;", Conn);
        cmd.Parameters.AddWithValue("StudentId", studentId);
        cmd.Parameters.AddWithValue("TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ledgerEntryIds.Add(reader.GetInt32(reader.GetOrdinal("id")));
        }

        return ledgerEntryIds;
    }

    /// <summary>
    /// Returns list of Exemption IDs
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public List<int> FindExemptionIDs(int studentId, int tenantId)
    {
        var exemptionIds = new List<int>();

        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT id
            FROM fee_exemption
            WHERE student_id = @StudentId AND tenant_id = @TenantId;", Conn);
        cmd.Parameters.AddWithValue("StudentId", studentId);
        cmd.Parameters.AddWithValue("TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            exemptionIds.Add(reader.GetInt32(reader.GetOrdinal("id")));
        }

        return exemptionIds;
    }

    /// <summary>
    /// Returns list of Enrollment Form IDs
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public List<int> FindEnrollmentFormIDs(int studentId, int tenantId)
    {
        var enrollmentFormIds = new List<int>();

        using var Conn = _database.GetConnection();
        using var cmd = new NpgsqlCommand(@"
            SELECT id
            FROM enrollment_form
            WHERE student_id = @StudentId AND tenant_id = @TenantId;", Conn);
        cmd.Parameters.AddWithValue("StudentId", studentId);
        cmd.Parameters.AddWithValue("TenantId", tenantId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            enrollmentFormIds.Add(reader.GetInt32(reader.GetOrdinal("id")));
        }

        return enrollmentFormIds;
    }
}
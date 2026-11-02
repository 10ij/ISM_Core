namespace ISM.Core;


// link tabble location:offering
public class Lesson
{
    public int ID { get; set; }

    public required int TenantID { get; set; }
    public required int LocationID { get; set; }
    public required int OfferingID { get; set; }

    // Move TeacherID to a seperate table, to track teacher changes

    public string? Notes { get; set; }

    public required DaysOfWeek DaysOfWeek { get; set; }
    
    public string? Room { get; set; }
    public decimal? DefaultPay { get; set; }
    public bool Archived { get; set; } = false;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}

public class Location
{
    public int ID { get; set; }

    public required int TenantID { get; set; }
    public required string Name { get; set; }

    public int? HeadTeacherID { get; set; }
    public int? DeputyID { get; set; }
    public int? SupervisorID { get; set; }

    public string? Telephone { get; set; }
    public string? Email { get; set; }
    public string? Address1 { get; set; }
    public string? Website { get; set; }

    public bool Archived { get; set; } = false;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    
}

public class Offering
{
    public int ID { get; set; }

    public required int TenantID { get; set; }
    public required string Name { get; set; }

    public string? Description { get; set; }

    public bool Archived { get; set; } = false;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}
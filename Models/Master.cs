namespace ISM.Core;

public class OrgSettings
{
    public int ID { get; set; }
    
    // OrgID is the ID that is exposed to the user
    // So they don't know how many tenants are stored in the system

    public required Guid OrgID { get; set; }

    public required string Name { get; set; }

    // These will be defualt values when makking a new location

    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }

    public string? Telephone { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public DateOnly ReportingPeriodStart { get; set; } = new DateOnly(DateTime.UtcNow.Year, 1, 1);

    // in months    
    public int ReportingPeriodLength { get; set; } = 3;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}
namespace ISM.Core;

public class Master
{
    public int ID { get; set; }
    
    // OrgID is the ID that is exposed to the user
    // So they don't know how many tenants are stored in the system

    // public required Guid OrgID { get; set; }

    // Instead of a GUID, I can use some cheap encryption algorithm
    // Before a Primary Key or Foreign Key is sent to the client over http
    // Its encrypted and then decrypted app side when running a query

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

    public bool IsArchived { get; set; } = false;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}
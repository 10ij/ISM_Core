namespace ISM.Core;

public class Teacher
{
    // This can be a headteacher, supervisor, deputy, whatever
    public int ID { get; set; }

    // Need to reference a config class, default to the tenants default title
    // Add an enum for titles in config class?
    public PersonTitle Title { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public string? HomeTelephone { get; set; }
    public required string MobileTelephone { get; set; }

    public required string Address { get; set; }

    public bool Archived { get; set; } = false;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    // Need a seperate DBS class/table 
}

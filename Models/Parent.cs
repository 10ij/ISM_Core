namespace ISM.Core;

public class Parent
{
    public int ID { get; set; }

    public required PersonTitle Title { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public string? HomeTelephone { get; set; }
    public required string MobileTelephone { get; set; }

    public required string Address { get; set; }

    public string? Notes { get; set; }

    public bool Archived { get; set; } = false;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}

// Link table parent:student
public class ParentStudent
{
    public int ID { get; set; }

    public required int StudentID { get; set; }

    public required int ParentID { get; set; }

    public required Relation Relationship { get; set; }

    // Main code should check, does student x already have a parent with this relationship?
    // Only Mother and Father are unique vals
    // Main code should check, does student x already have a relationship with said Order value?
    // No two records should have the same order, its the order of priority for guardianship
    public required int Order { get; set; }

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}
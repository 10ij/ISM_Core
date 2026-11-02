namespace ISM.Core;

public class Student
{
    public int ID { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public Gender Gender { get; set; }

    // A question marks means it can be set as Null rather than defaulting to some random date
    public DateOnly? DoB { get; set; }

    // Previous system had parents in a student record, what if one parent has multiple enrolled kids?
    // public required int ParentGrpID { get; set; }

    // Rather than defining a students class here, I'll put it in an enrollments class/table
    // If its just in the students row then how will a student have one profile across multiple classes?

    // public required int ClassID { get; set; }

    // public required DateTime StartDate { get; set; }

    // public DateTime? LeftDate { get; set; }

    // public required string? FormAttachmentID { get; set; }

    // The same goes for the above as they're per class data too

    // Only settable on creation, unlike access app
    // Unless you have db access ofc
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

    public string? Notes { get; set; }

    public string? Allergies { get; set; }

    public string? MedicalConditions { get; set; }

    public string? Telephone { get; set; }

}

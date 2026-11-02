namespace ISM.Core;

// Essentially a link table between student and lesson
public class Enrollment
{
    public int ID { get; set; }

    public required int StudentID { get; set; }
    public required int LessonID { get; set; }

    public required int StatusID { get; set; }
    
    public DateOnly? DateEnded { get; set; }

    public DateTime? DateModified { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}

public class EnrollmentStatus
{
    public int ID { get; set; }

    public required string Name { get; set; }
    
    public bool StopFeeAccrual { get; set; } = false;

    public string? Note { get; set; }

    public DateTime? DateModified { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}
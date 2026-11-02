namespace ISM.Core;

public class TeachingAssignment
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required int TeacherID { get; set; }
    public required int LessonID { get; set; }
    public bool IsSubstitute { get; set; } = false;
    public DateOnly? SubstituteDate { get; set; }
    public DateOnly? DateStarted { get; set; }
    public DateOnly? DateEnded { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}

public class TeachingAssignmentPay
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required int TeachingAssignmentID { get; set; }
    public required decimal LessonRate { get; set; }
    public DateOnly DateStarted { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? DateEnded { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}

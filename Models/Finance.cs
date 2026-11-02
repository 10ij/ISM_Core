namespace ISM.Core;

public enum BalanceEffect
{
    Debit,
    Credit
}

public class FeeExemption
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public decimal? FixedAmount { get; set; }
    public decimal? PercentageAmount { get; set; }
    public bool IsRecurring { get; set; } = false;
    public string? Note { get; set; }
    public DateOnly DateStarted { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? DateEnded { get; set; }
    public required int EnrollmentID { get; set; }
    public bool Archived { get; set; } = false;
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}

public class StudentEntryType
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required BalanceEffect AffectOnBalance { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}

public class LessonFee
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required int LessonID { get; set; }
    public required decimal Amount { get; set; }
    public DateOnly DateStarted { get; init; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? DateEnded { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}

public class StudentLedger
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required int StudentID { get; set; }
    public int? LessonFeeID { get; set; }
    public required decimal Amount { get; set; }
    public int? EntryTypeID { get; set; }
    public bool IsRectification { get; set; } = false;
    public int? RectificationOf { get; set; }
    public string? Note { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}

public class TeacherEntryType
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required BalanceEffect AffectOnBalance { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}

public class TeacherLedger
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required int TeacherID { get; set; }
    public required decimal Amount { get; set; }
    public int? TeachingAssignmentPayID { get; set; }
    public int? EntryTypeID { get; set; }
    public bool IsRectification { get; set; } = false;
    public int? RectificationOf { get; set; }
    public string? Note { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}

public class GeneralEntryType
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required BalanceEffect AffectOnBalance { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
    public DateTime DateModified { get; set; } = DateTime.UtcNow;
}

public class GeneralLedger
{
    public int ID { get; set; }
    public required int TenantID { get; set; }
    public required decimal Amount { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int? EntryTypeID { get; set; }
    public bool IsRectification { get; set; } = false;
    public int? RectificationOf { get; set; }
    public string? Note { get; set; }
    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}

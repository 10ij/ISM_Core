namespace ISM.Core;

// Shift credit/debit to per student. Administrators can filter by parent anyways and see total student dues
public class StudentLedger
{
    public int ID { get; set; }

    public required int StudentID { get; set; }

    public required LedgerEntryType Type { get; set; }

    public required decimal Amount { get; set; }

    public int? OfferingFeeID { get; set; }

    public string? Note { get; set; }

    public string? Reference { get; set; }

    public required DateOnly Date { get; set; }

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}

public class FeeExemption
{
    public int ID { get; set; }

    public required int StudentID { get; set; }

    public decimal? Percentage { get; set; }

    public bool IsRecurring { get; set; } = false;

    public int? EnrollmentID { get; set; }

    public decimal? FixedAmount { get; set; }

    public bool Archived { get; set; } = false;

    public string? Note { get; set; }

    public required DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;
}

public class IncomeExpenseType
{
    public int ID { get; set; }

    public required string Name { get; set; }

    public string? Note { get; set; }

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}

public class IncomeExpense
{
    public int ID { get; set; }

    public required int IncomeExpenseTypeID { get; set; }

    public required decimal Amount { get; set; }

    public string? Note { get; set; }

    public required DateOnly DateOccured { get; set; }

    public int? LocationID { get; set; }

    public DateTime DateModified { get; set; } = DateTime.UtcNow;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}

public class OfferingFee
{
    public int ID { get; set; }

    public required int OfferingID { get; set; }

    public required decimal FeeRate { get; set; }

    public DateOnly? DateEnded { get; set; }

    public required DateTime DateCreated { get; init; } = DateTime.UtcNow;

}
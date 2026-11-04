namespace ISM.Core;


// No need for file path, as file uploads are stored in a set location
// Under the files ID
public class EnrollmentForm
{
    public int ID { get; set; }

    public required int TenantID { get; set; }

    public required string Name { get; set; }

    public required int EnrollmentID { get; set; }

    public string? Note { get; set; }

    public required int StudentID { get; set; }

    public bool UploadComplete { get; set; } = false;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}
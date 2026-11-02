namespace ISM.Core;


// No need for file path, as file uploads are stored in a set location
// Under the files ID
public class Attachment
{
    public int ID { get; set; }

    public required string Name { get; set; }

    public string? Note { get; set; }

    public bool UploadComplete { get; set; } = false;

    public DateTime DateCreated { get; init; } = DateTime.UtcNow;

}

// DBS Check, extend attatchement

public class DBS : Attachment
{
    public required int TeacherID { get; set; }
}

// Form, extend attatchement

public class EnrollmentForm : Attachment
{
    public required int StudentID { get; set; }
}
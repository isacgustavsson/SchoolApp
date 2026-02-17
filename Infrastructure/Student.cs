
public class Student
{
    public int StudentId { get; set; }
    public string? Name { get; set; }

    // Navigation property for many-to-many relationship with courses
    public List<Course> Courses { get; set; } = new List<Course>();
}


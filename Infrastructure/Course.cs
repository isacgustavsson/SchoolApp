
public class Course
{
    public int CourseId { get; set; }
    public string? Title { get; set; }

    // Navigation property for many-to-many relationship with students
    public List<Student> Students { get; set; } = new List<Student>();
}


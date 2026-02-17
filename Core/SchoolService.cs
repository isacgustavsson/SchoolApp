using SchoolApp.Core.Entities;

namespace SchoolApp.Core;

public class SchoolService(SchoolDbContext db)
{
    private readonly SchoolDbContext _db = db;

    public List<Student> GetAllStudents()
    { }
    public List<Course> GetAllCourses()
    { }
    public void AddStudent(Student student)
    { }
    public void AddCourse(Course course)
    { }
    public void EnrollStudent(int studentId, int courseId)
    { }
}
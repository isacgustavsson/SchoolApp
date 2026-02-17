using SchoolApp.Core.Entities;
using SchoolApp.Core.Interfaces;


namespace SchoolApp.Core.Services;

public class SchoolService(IRepository repo)
{
    private readonly IRepository _repo = repo;

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
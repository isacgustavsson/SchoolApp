using SchoolApp.Core.Entities;

namespace SchoolApp.Core.Interfaces;

public interface IRepository
{
    public void Save();
    public Course? GetCourseById(int id);
    public Student? GetStudentById(int id);
    public List<Student> GetAllStudent();
    public List<Course> GetAllCourses();
    public void AddStudent(Student student);
    public void AddCourse(Course course);
}
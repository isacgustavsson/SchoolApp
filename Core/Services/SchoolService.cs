using SchoolApp.Core.Entities;
using SchoolApp.Core.Interfaces;


namespace SchoolApp.Core.Services;

public class SchoolService(IRepository repo)
{
    private readonly IRepository _repo = repo;

    public List<Student> GetAllStudents()
    {
        return _repo.GetAllStudent();
    }
    public List<Course> GetAllCourses()
    {
        return _repo.GetAllCourses();
    }
    public void AddStudent(string? name)
    {
        var newStudent = new Student
        { Name = name };

        _repo.AddStudent(newStudent);
    }
    public void AddCourse(string? title)
    {
        var course = new Course { Title = title };

        _repo.AddCourse(course);
    }
    public void HandleEnrollment(int studentId, int courseId)
    {
        var student = _repo.GetStudentById(studentId);
        var course = _repo.GetCourseById(courseId);

        bool success = HandleEnrollmentErrors(student, course);

        if (!success)
        {
            return;
        }

        course!.Students.Add(student!);
        _repo.Save();
    }

    static bool HandleEnrollmentErrors(Student? student, Course? course)
    {
        bool success = true;
        Console.ForegroundColor = ConsoleColor.Red;
        if (student == null || course == null)
        {
            Console.WriteLine("Student eller kurs hittades inte.");
            success = false;
        }
        else if (course.Students.Contains(student))
        {
            Console.WriteLine("Studenten är redan inskriven i kursen.");
            success = false;
        }
        else if (course.Students.Count >= 30)
        {
            Console.WriteLine("Kursen är full.");
            success = false;
        }
        Console.ResetColor();
        return success;
    }
}
using Microsoft.EntityFrameworkCore;
using SchoolApp.Core.Entities;
using SchoolApp.Core.Interfaces;

namespace SchoolApp.Infrastructure;

public class SchoolRepository(SchoolDbContext db) : IRepository
{
    private readonly SchoolDbContext _db = db;

    public void Save()
    {
        _db.SaveChanges();
    }

    public Course? GetCourseById(int id)
    {
        return _db.Courses
        .Include(c => c.Students)
        .FirstOrDefault(c => c.CourseId == id);
    }

    public Student? GetStudentById(int id)
    {
        return _db.Students
            .Include(s => s.Courses)
            .FirstOrDefault(s => s.StudentId == id);
    }

    public List<Student> GetAllStudent()
    {
        return _db.Students.ToList();
    }

    public List<Course> GetAllCourses()
    {
        return _db.Courses.ToList();
    }

    public void AddStudent(Student student)
    {
        _db.Add(student);
        _db.SaveChanges();
    }

    public void AddCourse(Course course)
    {
        _db.Add(course);
        _db.SaveChanges();
    }
}
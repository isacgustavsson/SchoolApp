using SchoolApp.Core.Services;

namespace SchoolApp.UI;

public class UI(SchoolService service)
{
    private readonly SchoolService _service = service;

    public void Run()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n SKOLSYSTEMET");
            Console.WriteLine("Lägg till [s]tudent");
            Console.WriteLine("Lägga till [k]urs");
            Console.WriteLine("[L]ägga till student i kurs");
            Console.WriteLine("[V]isa studenter och kurser");
            Console.WriteLine("[A]vsluta");
            Console.Write("Val: ");

            var answer = Console.ReadLine()!.ToLower();

            if (answer == "s")
            {
                AddStudent(_service);

            }
            else if (answer == "k")
            {
                AddCourse(_service);
            }
            else if (answer == "l")
            {
                EnrollStudent(_service);
            }
            else if (answer == "v")
            {
                ListStudentsAndCourses(_service);
            }
            else if (answer == "a")
            {
                break;
            }
        }
    }

    static void AddStudent(SchoolService _service)
    {
        Console.WriteLine("Ange studentens namn:");
        var name = Console.ReadLine();
        var student = new Student { Name = name };

        _service.Students.Add(student);
        _service.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Studenten {name} tillagd.");
        Console.ResetColor();
    }

    static void AddCourse(SchoolService _service)
    {
        Console.WriteLine("Ange kursens namn:");
        var title = Console.ReadLine();
        var course = new Course { Title = title };

        _service.Courses.Add(course);
        _service.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Kursen {title} tillagd.");
        Console.ResetColor();
    }

    static void EnrollStudent(SchoolService _service)
    {
        Console.WriteLine("Ange studentens id:");
        var studentId = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Ange kursens id:");
        var courseId = int.Parse(Console.ReadLine()!);

        var student = _service.Students.Find(studentId);
        var course = _service.Courses.Where(c => c.CourseId == courseId).Include(c => c.Students).FirstOrDefault();

        bool success = HandleEnrollmentErrors(student, course);
        if (!success)
        {
            return;
        }

        course!.Students.Add(student!);
        _service.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Studenten {student!.Name} tillagd i kursen {course.Title}.");
        Console.ResetColor();
    }

    static void ListStudentsAndCourses(SchoolService _service)
    {
        Console.WriteLine("STUDENTER:");

        foreach (var s in _service.Students.Include(s => s.Courses))
        {
            Console.Write($"\nID {s.StudentId}: {s.Name} ");
            foreach (var c in s.Courses)
            {
                Console.Write($"- {c.Title}");
            }
        }

        Console.WriteLine("\nKURSER:");
        foreach (var c in _service.Courses.Include(c => c.Students))
        {
            Console.WriteLine($"ID {c.CourseId}: {c.Title}");
        }
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
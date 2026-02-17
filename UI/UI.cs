using SchoolApp.Core.Services;
using SchoolApp.Core.Entities;

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
        string? name = Console.ReadLine();

        _service.AddStudent(name);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Studenten {name} tillagd.");
        Console.ResetColor();
    }

    static void AddCourse(SchoolService _service)
    {
        Console.WriteLine("Ange kursens namn:");
        string? title = Console.ReadLine();

        _service.AddCourse(title);

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

        _service.HandleEnrollment(studentId, courseId);

        // Console.ForegroundColor = ConsoleColor.Green;
        // Console.WriteLine($"Studenten {student!.Name} tillagd i kursen {course!.Title}.");
        // Console.ResetColor();
    }

    static void ListStudentsAndCourses(SchoolService _service)
    {
        Console.WriteLine("STUDENTER:");

        var students = _service.GetAllStudents();

        foreach (var s in students)
        {
            Console.Write($"\nID {s.StudentId}: {s.Name} ");
            foreach (var c in s.Courses)
            {
                Console.Write($"- {c.Title}");
            }
        }

        Console.WriteLine("\nKURSER:");

        var courses = _service.GetAllCourses();

        foreach (var c in courses)
        {
            Console.WriteLine($"ID {c.CourseId}: {c.Title}");
        }
    }
}
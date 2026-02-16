using Microsoft.EntityFrameworkCore;

using var db = new SchoolDbContext();

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
        AddStudent(db);

    }
    else if (answer == "k")
    {
        AddCourse(db);
    }
    else if (answer == "l")
    {
        EnrollStudent(db);
    }
    else if (answer == "v")
    {
        ListStudentsAndCourses(db);
    }
    else if (answer == "a")
    {
        break;
    }
}

static void AddStudent(SchoolDbContext db)
{
    Console.WriteLine("Ange studentens namn:");
    var name = Console.ReadLine();
    var student = new Student { Name = name };
    db.Students.Add(student);
    db.SaveChanges();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Studenten {name} tillagd.");
    Console.ResetColor();
}

static void AddCourse(SchoolDbContext db)
{
    Console.WriteLine("Ange kursens namn:");
    var title = Console.ReadLine();
    var course = new Course { Title = title };
    db.Courses.Add(course);
    db.SaveChanges();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Kursen {title} tillagd.");
    Console.ResetColor();
}

static void EnrollStudent(SchoolDbContext db)
{
    Console.WriteLine("Ange studentens id:");
    var studentId = int.Parse(Console.ReadLine()!);
    Console.WriteLine("Ange kursens id:");
    var courseId = int.Parse(Console.ReadLine()!);
    var student = db.Students.Find(studentId);
    var course = db.Courses.Where(c => c.CourseId == courseId).Include(c => c.Students).FirstOrDefault();

    bool success = HandleEnrollmentErrors(student, course);
    if (!success)
    {
        return;
    }

    course!.Students.Add(student!);
    db.SaveChanges();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Studenten {student!.Name} tillagd i kursen {course.Title}.");
    Console.ResetColor();
}

static void ListStudentsAndCourses(SchoolDbContext db)
{
    Console.WriteLine("STUDENTER:");
    foreach (var s in db.Students.Include(s => s.Courses))
    {
        Console.Write($"\nID {s.StudentId}: {s.Name} ");
        foreach (var c in s.Courses)
        {
            Console.Write($"- {c.Title}");
        }
    }

    Console.WriteLine("\nKURSER:");
    foreach (var c in db.Courses.Include(c => c.Students))
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
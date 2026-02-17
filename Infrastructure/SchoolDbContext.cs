using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Infrastructure;

public class SchoolDbContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Konfigurera SQLite som databasleverantör.
        optionsBuilder.UseSqlite("Data Source=school.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed data för studenter
        modelBuilder.Entity<Student>().HasData(
            new Student { StudentId = 1, Name = "Alice" },
            new Student { StudentId = 2, Name = "Bob" }
        );

        // Seed data för kurser
        modelBuilder.Entity<Course>().HasData(
            new Course { CourseId = 1, Title = "Mathematics" },
            new Course { CourseId = 2, Title = "History" }
        );

        // Seed many-to-many-relationen: Alice är inskriven i Mathematics
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity(j => j.HasData(
                new { StudentsStudentId = 1, CoursesCourseId = 1 }
            ));
    }
}


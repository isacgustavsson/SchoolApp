using SchoolApp.Core.Interfaces;

namespace SchoolApp.Infrastructure;

public class SchoolRepository(SchoolDbContext db) : IRepository
{
    private readonly SchoolDbContext _db = db;
    public void Add()
    { }
}
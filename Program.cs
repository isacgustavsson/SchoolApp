using Microsoft.EntityFrameworkCore;

using var db = new SchoolDbContext();

SchoolService service = new();
UI schoolUI = new(service);

schoolUI.Run();
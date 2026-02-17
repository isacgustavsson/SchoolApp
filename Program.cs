using Microsoft.EntityFrameworkCore;

using var db = new SchoolDbContext();

SchoolService service = new(db);
UI schoolUI = new(service);

schoolUI.Run();
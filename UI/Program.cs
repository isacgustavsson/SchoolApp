using Microsoft.EntityFrameworkCore;
using SchoolApp.UI;
using SchoolApp.Core;
using SchoolApp.Infrastructure;

var db = new SchoolDbContext();

SchoolService service = new(db);
UI schoolUI = new(service);

schoolUI.Run();
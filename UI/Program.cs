using SchoolApp.UI;
using SchoolApp.Core.Services;
using SchoolApp.Infrastructure;

var db = new SchoolDbContext();
var repo = new SchoolRepository(db);

SchoolService service = new(repo);
UI schoolUI = new(service);

schoolUI.Run();
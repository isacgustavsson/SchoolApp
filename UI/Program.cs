using SchoolApp.UI;
using SchoolApp.Core.Services;
using SchoolApp.Infrastructure;

var db = new SchoolDbContext();
db.Database.EnsureCreated();

var repo = new SchoolRepository(db);
var service = new SchoolService(repo);
var ui = new UI(service);

ui.Run();
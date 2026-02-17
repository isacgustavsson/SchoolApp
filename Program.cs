using Microsoft.EntityFrameworkCore;

using var db = new SchoolDbContext();

UI schoolUI = new();
schoolUI.Run();
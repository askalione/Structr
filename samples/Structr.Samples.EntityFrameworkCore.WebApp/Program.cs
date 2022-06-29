using Structr.Samples.EntityFrameworkCore.WebApp.DataAccess;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddSqlServer<AppDbContext>("Data Source=.\\SQLEXPRESS;Initial Catalog=EFCore;Integrated Security=True");
services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

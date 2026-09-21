using Microsoft.EntityFrameworkCore;
using WorkSphere.DepartmentService.Data;
using WorkSphere.DepartmentService.Repositories;
using WorkSphere.DepartmentService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();



builder.Services.AddSwaggerGen();

// Register DepartmentDbContext with SQL Server
builder.Services.AddDbContext<DepartmentDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Register Repository and Service
builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<DepartmentService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DepartmentDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

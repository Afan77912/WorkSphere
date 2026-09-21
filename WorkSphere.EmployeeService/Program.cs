
using Microsoft.EntityFrameworkCore;
using WorkSphere.EmployeeService.Clients;
using WorkSphere.EmployeeService.Data;
using WorkSphere.EmployeeService.Messaging;
using WorkSphere.EmployeeService.Middleware;
using WorkSphere.EmployeeService.Repositories;
using WorkSphere.EmployeeService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "redis:6379";
    options.InstanceName = "WorkSphere:"; 
});

builder.Services.AddSingleton<RedisCacheService>();
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://localhost:7060")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<RabbitMqPublisher>();

// Register DepartmentClient for Department Service communication
builder.Services.AddHttpClient<DepartmentClient>(client =>
{
    client.BaseAddress = new Uri("http://departmentservice:8080/");
});

// Register EmployeeDbContext with SQL Server
builder.Services.AddDbContext<EmployeeDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors("AllowFrontend");

// Global Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();


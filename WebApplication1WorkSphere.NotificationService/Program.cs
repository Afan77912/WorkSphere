using WorkSphere.NotificationService.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

    builder.Services.AddSingleton<NotificationConsumer>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var consumer = app.Services.GetRequiredService<NotificationConsumer>();
await consumer.StartAsync();

app.Run();
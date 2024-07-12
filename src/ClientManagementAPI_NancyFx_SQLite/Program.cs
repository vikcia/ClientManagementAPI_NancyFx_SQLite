using Application;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add serilog services to the container and read config from appsettings
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Serilog for logging
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbInitializer = services.GetRequiredService<IDatabaseBootstrap>();
        dbInitializer.Setup();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while setting up the database: {ex.Message}");
    }
}

app.Run();
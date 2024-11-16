using DataAccess;
using DataAccess.Entities;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.MeterReadings;
using WebApi.MeterReadings.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints().SwaggerDocument();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration["DbConnectionString"]);
});

builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();

builder.Services.AddScoped<IValidator<MeterReading>, MeterReadingValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await DevEnvironmentOnlyDatabaseSetup(app);
}

app.UseFastEndpoints().UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();

static async Task DevEnvironmentOnlyDatabaseSetup(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
    if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
    {
        await dbContext.Database.MigrateAsync();
    }
}
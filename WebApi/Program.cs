using DataAccess;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApi.Contracts.Services;
using WebApi.MeterReadings;
using WebApi.MeterReadings.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration["DbConnectionString"]);
});

builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await DevEnvironmentOnlyDatabaseSetup(app);

    //TODO change to app.UseFastEndpoints().UseSwagger()
    //app.UseFastEndpoints().UseSwaggerUI() ?
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFastEndpoints();

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
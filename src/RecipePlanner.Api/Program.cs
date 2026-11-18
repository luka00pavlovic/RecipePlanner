using Microsoft.EntityFrameworkCore;
using RecipePlanner.Application;
using RecipePlanner.Infrastructure;
using RecipePlanner.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("RecipePlanner") ?? throw new InvalidOperationException("Connection string 'RecipePlanner' not found."));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<RecipePlannerDbContext>().Database.Migrate();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

// Feature endpoints (recipes, meal plans, shopping list) are added in later milestones — see docs/SCOPE.md.

app.Run();

// Exposes Program to WebApplicationFactory in the integration tests.
public partial class Program { }

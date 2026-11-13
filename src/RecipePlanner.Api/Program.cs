using RecipePlanner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("RecipePlanner") ?? "Data Source=recipeplanner.db");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

// Feature endpoints (recipes, meal plans, shopping list) are added in later milestones — see docs/SCOPE.md.

app.Run();

// Exposes Program to WebApplicationFactory in the integration tests.
public partial class Program { }

# Recipe Planner

[![CI](https://github.com/luka00pavlovic/RecipePlanner/actions/workflows/ci.yml/badge.svg)](https://github.com/luka00pavlovic/RecipePlanner/actions/workflows/ci.yml)

Store your recipes, plan the week's meals, and get a shopping list generated automatically.
Built with **.NET 10**, **ASP.NET Core**, **Blazor** and **EF Core**.

> 🚧 Work in progress — see [docs/SCOPE.md](docs/SCOPE.md) for the roadmap.

## Features

- [ ] Recipe management (CRUD, search, serving scaling)
- [ ] Weekly meal planner
- [ ] Generated shopping list with merged ingredients
- [ ] Blazor web UI

## Project structure

```
src/
  RecipePlanner.Domain/          Entities and business rules
  RecipePlanner.Application/     Use cases, DTOs, validation
  RecipePlanner.Infrastructure/  EF Core DbContext, SQLite
  RecipePlanner.Api/             ASP.NET Core Minimal API
  RecipePlanner.Web/             Blazor front end
tests/
  RecipePlanner.UnitTests/
  RecipePlanner.IntegrationTests/
```

## Getting started

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/RecipePlanner.Api     # API + OpenAPI at /openapi/v1.json
dotnet run --project src/RecipePlanner.Web     # Blazor UI
```


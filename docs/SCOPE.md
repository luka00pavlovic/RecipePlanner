# Recipe Planner — Project Scope

## Goal

A web app where a user keeps their recipes, plans meals for the week, and gets a
generated shopping list. Built as a portfolio project to show clean, tested,
production-style C#/.NET code, not to compete with commercial apps.

## Tech stack

| Area          | Choice                                              |
|---------------|-----------------------------------------------------|
| Runtime       | .NET 10 (LTS), C# 14                                |
| API           | ASP.NET Core Minimal APIs + OpenAPI                  |
| Front end     | Blazor Web App (interactive server)                  |
| Data          | EF Core 10 + SQLite (swap to PostgreSQL later)       |
| Tests         | xUnit, WebApplicationFactory integration tests       |
| CI            | GitHub Actions (build + test on every push / PR)     |

## Architecture

```
RecipePlanner.Web ──HTTP──▶ RecipePlanner.Api
                                  │
                     RecipePlanner.Infrastructure  (EF Core, SQLite)
                                  │
                     RecipePlanner.Application     (use cases, DTOs, validation)
                                  │
                     RecipePlanner.Domain          (entities + business rules, no dependencies)
```

Dependencies point inward: Domain knows nothing about EF Core or HTTP.

## MVP (v1.0)

1. **Recipes CRUD** — name, description, servings, prep/cook time, instructions,
   ingredient lines (name, quantity, unit). Validation on create/update.
2. **Search & filter recipes** — by name, ingredient, and max total time.
3. **Serving scaling** — view a recipe scaled to N servings.
4. **Weekly meal plan** — assign recipes to a date + meal slot
   (breakfast / lunch / dinner / snack) with a serving count.
5. **Shopping list** — generated from a meal plan; merges identical
   ingredients with the same unit (e.g. 200 g + 150 g flour = 350 g flour).
6. **Blazor UI** for all of the above.
7. **Tests** — unit tests for domain rules, integration tests for each endpoint.
8. **CI** green on `main`, README with screenshots and setup steps.

## Stretch goals (v1.x) 

- User accounts (ASP.NET Core Identity) so each user has their own recipes
- Tags / categories (vegetarian, quick, dessert…) and favourites
- Recipe images (upload to local storage, later Azure Blob)
- Unit conversion in the shopping list (g ↔ kg, ml ↔ l)
- Import a recipe from a URL (parse schema.org Recipe JSON-LD)
- Nutrition totals per day using an external API
- Docker Compose with PostgreSQL; deploy to Azure App Service or Fly.io


## Milestones

| # | Milestone              | Done when                                                        |
|---|------------------------|------------------------------------------------------------------|
| 0 | Project setup          | Solution builds, CI green, scope + README committed ✅           |
| 1 | Recipes API            | CRUD + search endpoints, EF migration, integration tests         |
| 2 | Recipes UI             | List, detail (with scaling), create/edit forms in Blazor         |
| 3 | Meal plans             | API + weekly calendar view in the UI                             |
| 4 | Shopping list          | Generation + merging logic with unit tests, printable view       |
| 5 | Polish & ship          | Seed data, screenshots, error handling, deployed demo link       |
| 6 | Stretch                | Pick 1–2 stretch goals (Identity is the most valuable on a CV)   |

Each milestone maps to a GitHub milestone, with one issue per feature and one
branch + pull request per issue.

## Domain model (initial)

- **Recipe** — Id, Name, Description, Servings, PrepMinutes, CookMinutes,
  Instructions, Ingredients[] — rule: `ScaleTo(servings)`
- **RecipeIngredient** — Name, Quantity, Unit (value object)
- **MealPlan** — Id, Name, StartDate, EndDate, Entries[] — rule: entries must fall within the plan's dates
- **MealPlanEntry** — Id, RecipeId, Date, MealType, Servings
- **ShoppingList** — computed from a MealPlan, not stored (v1)

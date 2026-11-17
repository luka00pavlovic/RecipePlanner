namespace RecipePlanner.Application.Recipes;

/// <summary>A recipe as returned by the API.</summary>
public record RecipeResponse(
    Guid Id,
    string Name,
    string? Description,
    int Servings,
    int PrepMinutes,
    int CookMinutes,
    int TotalMinutes,
    string Instructions,
    IReadOnlyList<IngredientDto> Ingredients,
    DateTime CreatedAtUtc);

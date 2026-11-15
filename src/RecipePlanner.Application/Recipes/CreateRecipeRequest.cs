namespace RecipePlanner.Application.Recipes;

/// <summary>Body of POST /recipes.</summary>
public record CreateRecipeRequest(
    string Name,
    string? Description,
    int Servings,
    int PrepMinutes,
    int CookMinutes,
    string? Instructions,
    IReadOnlyList<IngredientDto> Ingredients);

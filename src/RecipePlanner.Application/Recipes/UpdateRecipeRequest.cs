namespace RecipePlanner.Application.Recipes;

/// <summary>Body of PUT /recipes/{id}. Replaces the whole recipe, including all ingredients.</summary>
public record UpdateRecipeRequest(
    string Name,
    string? Description,
    int Servings,
    int PrepMinutes,
    int CookMinutes,
    string? Instructions,
    IReadOnlyList<IngredientDto> Ingredients);

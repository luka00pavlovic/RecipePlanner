using System.Collections.Generic;

namespace RecipePlanner.Application.Recipes;

public interface IRecipeRequest
{
    string Name { get; }
    string? Description { get; }
    int Servings { get; }
    int PrepMinutes { get; }
    int CookMinutes { get; }
    string? Instructions { get; }
    IReadOnlyList<IngredientDto> Ingredients { get; }
}
namespace RecipePlanner.Domain.Recipes;

/// <summary>An ingredient line on a recipe, e.g. "200 g flour".</summary>
public record RecipeIngredient(string Name, decimal Quantity, string Unit);

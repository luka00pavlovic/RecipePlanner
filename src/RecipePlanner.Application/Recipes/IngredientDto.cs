namespace RecipePlanner.Application.Recipes;

/// <summary>An ingredient line as sent to and returned from the API, e.g. 200 g flour.</summary>
public record IngredientDto(string Name, decimal Quantity, string? Unit);

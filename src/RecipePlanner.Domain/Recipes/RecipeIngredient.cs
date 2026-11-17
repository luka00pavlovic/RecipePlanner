namespace RecipePlanner.Domain.Recipes;

/// <summary>An ingredient line on a recipe, e.g. "200 g flour".</summary>
public record RecipeIngredient
{
    public RecipeIngredient(string name, decimal quantity, string? unit)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit ?? string.Empty;
    }

    public string Name
    {
        get;
        init => field = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Ingredient name is required.", nameof(Name))
            : value.Trim();
    }
    public decimal Quantity
    {
        get;
        init => field = value > 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity must be positive.");
    }

    public string Unit
    {
        get;
        init => field = value.Trim().ToLowerInvariant();
    }
}
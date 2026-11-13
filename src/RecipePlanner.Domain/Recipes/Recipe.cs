namespace RecipePlanner.Domain.Recipes;

public class Recipe
{
    private readonly List<RecipeIngredient> _ingredients = [];

    private Recipe() { } // EF Core

    public Recipe(string name, int servings, string? description = null)
    {
        Rename(name);
        SetServings(servings);
        Description = description;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Servings { get; private set; }
    public int PrepMinutes { get; set; }
    public int CookMinutes { get; set; }
    public string Instructions { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public IReadOnlyList<RecipeIngredient> Ingredients => _ingredients;

    public int TotalMinutes => PrepMinutes + CookMinutes;

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Recipe name is required.", nameof(name));
        Name = name.Trim();
    }

    public void SetServings(int servings)
    {
        if (servings <= 0)
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be positive.");
        Servings = servings;
    }

    public void AddIngredient(string name, decimal quantity, string unit) =>
        _ingredients.Add(new RecipeIngredient(name, quantity, unit));

    /// <summary>Returns the ingredient list scaled to a different number of servings.</summary>
    public IReadOnlyList<RecipeIngredient> ScaleTo(int servings)
    {
        if (servings <= 0)
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be positive.");

        var factor = (decimal)servings / Servings;
        return _ingredients
            .Select(i => i with { Quantity = Math.Round(i.Quantity * factor, 2) })
            .ToList();
    }
}

namespace RecipePlanner.Domain.Recipes;

public class Recipe
{
    private readonly List<RecipeIngredient> _ingredients = [];

    private Recipe() { } // EF Core

    public Recipe(string name, int servings, string? description = null)
    {
        Rename(name);
        SetServings(servings);
        SetDescription(description);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int Servings { get; private set; }
    public int PrepMinutes { get; private set; }
    public int CookMinutes { get; private set; }
    public string Instructions { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public IReadOnlyList<RecipeIngredient> Ingredients => _ingredients;

    public int TotalMinutes => PrepMinutes + CookMinutes;

    public void SetTimes(int prepMinutes, int cookMinutes)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(prepMinutes);
        ArgumentOutOfRangeException.ThrowIfNegative(cookMinutes);
        PrepMinutes = prepMinutes;
        CookMinutes = cookMinutes;
    }

    public void SetInstructions(string? instructions) =>
        Instructions = instructions?.Trim() ?? string.Empty;

    public void SetDescription(string? description) =>
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

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

    public void ReplaceIngredients(IEnumerable<RecipeIngredient> ingredients)
    {
        _ingredients.Clear();
        _ingredients.AddRange(ingredients);
    }

    /// <summary>Returns the ingredient list scaled to a different number of servings.</summary>
    public IReadOnlyList<RecipeIngredient> ScaleTo(int servings)
    {
        if (servings <= 0)
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be positive.");

        return _ingredients
            .Select(i => i with { Quantity = i.Quantity * servings / Servings })
            .ToList();
    }
}

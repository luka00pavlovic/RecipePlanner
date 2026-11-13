namespace RecipePlanner.Domain.MealPlans;

public enum MealType
{
    Breakfast,
    Lunch,
    Dinner,
    Snack
}

public class MealPlanEntry
{
    private MealPlanEntry() { } // EF Core

    internal MealPlanEntry(Guid recipeId, DateOnly date, MealType mealType, int servings)
    {
        if (servings <= 0)
            throw new ArgumentOutOfRangeException(nameof(servings), "Servings must be positive.");
        RecipeId = recipeId;
        Date = date;
        MealType = mealType;
        Servings = servings;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid RecipeId { get; private set; }
    public DateOnly Date { get; private set; }
    public MealType MealType { get; private set; }
    public int Servings { get; private set; }
}

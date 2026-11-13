namespace RecipePlanner.Domain.MealPlans;

/// <summary>A week (or any date range) of planned meals.</summary>
public class MealPlan
{
    private readonly List<MealPlanEntry> _entries = [];

    private MealPlan() { } // EF Core

    public MealPlan(string name, DateOnly startDate, int days = 7)
    {
        if (days is < 1 or > 31)
            throw new ArgumentOutOfRangeException(nameof(days), "A plan must cover 1–31 days.");
        Name = name;
        StartDate = startDate;
        EndDate = startDate.AddDays(days - 1);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }

    public IReadOnlyList<MealPlanEntry> Entries => _entries;

    public MealPlanEntry Schedule(Guid recipeId, DateOnly date, MealType mealType, int servings)
    {
        if (date < StartDate || date > EndDate)
            throw new ArgumentOutOfRangeException(nameof(date), "Date is outside this meal plan.");

        var entry = new MealPlanEntry(recipeId, date, mealType, servings);
        _entries.Add(entry);
        return entry;
    }

    public bool Remove(Guid entryId) => _entries.RemoveAll(e => e.Id == entryId) > 0;
}

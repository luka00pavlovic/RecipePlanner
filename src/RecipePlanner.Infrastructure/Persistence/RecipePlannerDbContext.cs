using Microsoft.EntityFrameworkCore;
using RecipePlanner.Domain.MealPlans;
using RecipePlanner.Domain.Recipes;

namespace RecipePlanner.Infrastructure.Persistence;

public class RecipePlannerDbContext(DbContextOptions<RecipePlannerDbContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<MealPlan> MealPlans => Set<MealPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>(recipe =>
        {
            recipe.HasKey(r => r.Id);
            recipe.Property(r => r.Name).HasMaxLength(200).IsRequired();
            recipe.Ignore(r => r.TotalMinutes);
            recipe.OwnsMany(r => r.Ingredients, ingredient =>
            {
                ingredient.WithOwner().HasForeignKey("RecipeId");
                ingredient.Property<int>("Id");
                ingredient.HasKey("Id");
                ingredient.Property(i => i.Name).HasMaxLength(100).IsRequired();
                ingredient.Property(i => i.Unit).HasMaxLength(20);
            });
            recipe.Navigation(r => r.Ingredients).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<MealPlan>(plan =>
        {
            plan.HasKey(p => p.Id);
            plan.Property(p => p.Name).HasMaxLength(200).IsRequired();
            plan.HasMany(p => p.Entries).WithOne().HasForeignKey("MealPlanId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            plan.Navigation(p => p.Entries).UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<MealPlanEntry>(entry =>
        {
            entry.HasKey(e => e.Id);
            entry.Property(e => e.MealType).HasConversion<string>();
            entry.HasOne<Recipe>().WithMany().HasForeignKey(e => e.RecipeId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}

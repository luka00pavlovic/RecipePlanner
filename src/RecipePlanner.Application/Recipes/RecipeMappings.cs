using RecipePlanner.Domain.Recipes;

namespace RecipePlanner.Application.Recipes;

/// <summary>
/// Maps between recipe DTOs and the <see cref="Recipe"/> entity.
/// Requests should be validated first; the domain still throws if invalid data gets through.
/// </summary>
public static class RecipeMappings
{
    public static Recipe ToRecipe(this CreateRecipeRequest request)
    {
        var recipe = new Recipe(request.Name, request.Servings, request.Description);
        recipe.SetTimes(request.PrepMinutes, request.CookMinutes);
        recipe.SetInstructions(request.Instructions);
        recipe.ReplaceIngredients(request.Ingredients.Select(ToIngredient).ToList());
        return recipe;
    }

    /// <summary>Updates an existing (tracked) recipe in place, so it keeps its Id.</summary>
    public static void ApplyUpdate(this Recipe recipe, UpdateRecipeRequest request)
    {
        recipe.Rename(request.Name);
        recipe.SetServings(request.Servings);
        recipe.SetDescription(request.Description);
        recipe.SetTimes(request.PrepMinutes, request.CookMinutes);
        recipe.SetInstructions(request.Instructions);
        recipe.ReplaceIngredients(request.Ingredients.Select(ToIngredient).ToList());
    }

    public static RecipeResponse ToResponse(this Recipe recipe) => new(
        recipe.Id,
        recipe.Name,
        recipe.Description,
        recipe.Servings,
        recipe.PrepMinutes,
        recipe.CookMinutes,
        recipe.TotalMinutes,
        recipe.Instructions,
        recipe.Ingredients.Select(ToDto).ToList(),
        recipe.CreatedAtUtc);

    public static IngredientDto ToDto(this RecipeIngredient ingredient) =>
        new(ingredient.Name, ingredient.Quantity, ingredient.Unit);

    private static RecipeIngredient ToIngredient(IngredientDto dto) =>
        new(dto.Name, dto.Quantity, dto.Unit);
}

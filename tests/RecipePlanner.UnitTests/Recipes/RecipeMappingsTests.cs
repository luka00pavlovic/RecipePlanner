using RecipePlanner.Application.Recipes;
using RecipePlanner.Domain.Recipes;

namespace RecipePlanner.UnitTests.Recipes;

public class RecipeMappingsTests
{
    private static CreateRecipeRequest CreateRequest() => new(
        Name: "  Pancakes ",
        Description: "Fluffy",
        Servings: 2,
        PrepMinutes: 10,
        CookMinutes: 15,
        Instructions: "  Mix and fry. ",
        Ingredients: [new IngredientDto(" Flour ", 150m, " G "), new IngredientDto("Eggs", 2m, null)]);

    private static UpdateRecipeRequest UpdateRequest() => new(
        Name: "Oat pancakes",
        Description: "Healthier",
        Servings: 4,
        PrepMinutes: 5,
        CookMinutes: 20,
        Instructions: "Blend, rest, fry.",
        Ingredients: [new IngredientDto("Oats", 100m, "g"), new IngredientDto("Milk", 250m, "ml"), new IngredientDto("Banana", 1m, "pcs")]);

    [Fact]
    public void ToRecipe_CopiesAllFields()
    {
        var recipe = CreateRequest().ToRecipe();

        Assert.Equal("Pancakes", recipe.Name);
        Assert.Equal("Fluffy", recipe.Description);
        Assert.Equal(2, recipe.Servings);
        Assert.Equal(10, recipe.PrepMinutes);
        Assert.Equal(15, recipe.CookMinutes);
        Assert.Equal("Mix and fry.", recipe.Instructions);
        Assert.NotEqual(Guid.Empty, recipe.Id);
    }

    [Fact]
    public void ToRecipe_NormalisesIngredients()
    {
        var recipe = CreateRequest().ToRecipe();

        Assert.Collection(recipe.Ingredients,
            flour =>
            {
                Assert.Equal("Flour", flour.Name);
                Assert.Equal(150m, flour.Quantity);
                Assert.Equal("g", flour.Unit);
            },
            eggs =>
            {
                Assert.Equal("Eggs", eggs.Name);
                Assert.Equal(2m, eggs.Quantity);
                Assert.Equal(string.Empty, eggs.Unit);
            });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    public void ToRecipe_StoresNullDescription_WhenBlank(string? description)
    {
        var recipe = (CreateRequest() with { Description = description }).ToRecipe();

        Assert.Null(recipe.Description);
    }

    [Fact]
    public void ToRecipe_Throws_WhenIngredientIsInvalid()
    {
        var request = CreateRequest() with { Ingredients = [new IngredientDto("Flour", 0m, "g")] };

        Assert.Throws<ArgumentOutOfRangeException>(() => request.ToRecipe());
    }

    [Fact]
    public void ApplyUpdate_ChangesAllFields_AndKeepsId()
    {
        var recipe = CreateRequest().ToRecipe();
        var originalId = recipe.Id;
        var originalCreatedAt = recipe.CreatedAtUtc;

        recipe.ApplyUpdate(UpdateRequest());

        Assert.Equal(originalId, recipe.Id);
        Assert.Equal(originalCreatedAt, recipe.CreatedAtUtc);
        Assert.Equal("Oat pancakes", recipe.Name);
        Assert.Equal("Healthier", recipe.Description);
        Assert.Equal(4, recipe.Servings);
        Assert.Equal(5, recipe.PrepMinutes);
        Assert.Equal(20, recipe.CookMinutes);
        Assert.Equal("Blend, rest, fry.", recipe.Instructions);
    }

    [Fact]
    public void ApplyUpdate_ReplacesIngredients()
    {
        var recipe = CreateRequest().ToRecipe();

        recipe.ApplyUpdate(UpdateRequest());

        Assert.Equal(["Oats", "Milk", "Banana"], recipe.Ingredients.Select(i => i.Name));
    }

    [Fact]
    public void ApplyUpdate_WithInvalidIngredient_LeavesIngredientsUnchanged()
    {
        var recipe = CreateRequest().ToRecipe();
        var request = UpdateRequest() with
        {
            Ingredients = [new IngredientDto("Oats", 100m, "g"), new IngredientDto("", 1m, "g")]
        };

        Assert.Throws<ArgumentException>(() => recipe.ApplyUpdate(request));

        Assert.Equal(["Flour", "Eggs"], recipe.Ingredients.Select(i => i.Name));
    }

    [Fact]
    public void ToResponse_CopiesAllFields_IncludingTotalMinutes()
    {
        var recipe = CreateRequest().ToRecipe();

        var response = recipe.ToResponse();

        Assert.Equal(recipe.Id, response.Id);
        Assert.Equal("Pancakes", response.Name);
        Assert.Equal("Fluffy", response.Description);
        Assert.Equal(2, response.Servings);
        Assert.Equal(10, response.PrepMinutes);
        Assert.Equal(15, response.CookMinutes);
        Assert.Equal(25, response.TotalMinutes);
        Assert.Equal("Mix and fry.", response.Instructions);
        Assert.Equal(recipe.CreatedAtUtc, response.CreatedAtUtc);
    }

    [Fact]
    public void ToResponse_MapsIngredientsInOrder()
    {
        var response = CreateRequest().ToRecipe().ToResponse();

        Assert.Equal(
            [new IngredientDto("Flour", 150m, "g"), new IngredientDto("Eggs", 2m, string.Empty)],
            response.Ingredients);
    }

    [Fact]
    public void ToDto_CopiesIngredient()
    {
        var dto = new RecipeIngredient("Sugar", 50m, "g").ToDto();

        Assert.Equal(new IngredientDto("Sugar", 50m, "g"), dto);
    }
}

using RecipePlanner.Domain.Recipes;

namespace RecipePlanner.UnitTests.Recipes;

public class RecipeTests
{
    [Fact]
    public void ScaleTo_DoublesQuantities_WhenServingsDouble()
    {
        var recipe = new Recipe("Pancakes", servings: 2);
        recipe.AddIngredient("Flour", 150m, "g");
        recipe.AddIngredient("Eggs", 1m, "pcs");

        var scaled = recipe.ScaleTo(4);

        Assert.Equal(300m, scaled[0].Quantity);
        Assert.Equal(2m, scaled[1].Quantity);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_Throws_WhenNameIsBlank(string name)
    {
        Assert.Throws<ArgumentException>(() => new Recipe(name, servings: 2));
    }

    [Fact]
    public void Constructor_Throws_WhenServingsNotPositive()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Recipe("Soup", servings: 0));
    }
    [Theory]
    [InlineData(-1, 10)]
    [InlineData(10, -1)]
    public void SetTimes_Throws_WhenNegative(int prep, int cook)
    {
        var recipe = new Recipe("Soup", servings: 2);

        Assert.Throws<ArgumentOutOfRangeException>(() => recipe.SetTimes(prep, cook));
    }

    [Fact]
    public void SetTimes_AllowsZero_AndTotalIsSum()
    {
        var recipe = new Recipe("Salad", servings: 2);

        recipe.SetTimes(15, 0);

        Assert.Equal(15, recipe.TotalMinutes);
    }

    [Fact]
    public void ReplaceIngredients_RemovesOld_AndAddsNew()
    {
        var recipe = new Recipe("Pancakes", servings: 2);
        recipe.AddIngredient("Flour", 150m, "g");

        recipe.ReplaceIngredients([new RecipeIngredient("Oat flour", 120m, "g"), new RecipeIngredient("Milk", 200m, "ml")]);

        Assert.Equal(["Oat flour", "Milk"], recipe.Ingredients.Select(i => i.Name));
    }

    [Fact]
    public void ScaleTo_DoesNotThrow_WhenScalingTinyQuantityDown()
    {
        var recipe = new Recipe("Soup", servings: 4);
        recipe.AddIngredient("Salt", 0.01m, "tsp");

        var scaled = recipe.ScaleTo(1);

        Assert.Equal(0.0025m, scaled[0].Quantity);
    }

    [Theory]
    [InlineData(2, 3, 150, 225)]   // x1.5
    [InlineData(3, 2, 3, 2)]       // x2/3, must stay exact
    public void ScaleTo_GivesExactQuantities(int from, int to, decimal quantity, decimal expected)
    {
        var recipe = new Recipe("Pancakes", servings: from);
        recipe.AddIngredient("Eggs", quantity, "pcs");

        Assert.Equal(expected, recipe.ScaleTo(to)[0].Quantity);
    }

    [Fact]
    public void SetInstructions_TrimsText_AndTreatsNullAsEmpty()
    {
        var recipe = new Recipe("Soup", servings: 2);

        recipe.SetInstructions("  Boil water.  ");
        Assert.Equal("Boil water.", recipe.Instructions);

        recipe.SetInstructions(null);
        Assert.Equal(string.Empty, recipe.Instructions);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    public void SetDescription_StoresNull_WhenBlank(string? description)
    {
        var recipe = new Recipe("Soup", servings: 2, description: "Old");

        recipe.SetDescription(description);

        Assert.Null(recipe.Description);
    }

}

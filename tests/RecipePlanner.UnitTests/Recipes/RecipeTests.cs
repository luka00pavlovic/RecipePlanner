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
}

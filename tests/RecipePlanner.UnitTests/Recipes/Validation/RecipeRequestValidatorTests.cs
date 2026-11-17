using FluentValidation.TestHelper;
using RecipePlanner.Application.Recipes;
using RecipePlanner.Application.Recipes.Validation;

namespace RecipePlanner.UnitTests.Recipes.Validation;

public class RecipeRequestValidatorTests
{
    private readonly CreateRecipeRequestValidator _validator = new();

    private static CreateRecipeRequest ValidRequest() => new(
        Name: "Pancakes",
        Description: "Fluffy breakfast pancakes",
        Servings: 2,
        PrepMinutes: 10,
        CookMinutes: 15,
        Instructions: "Mix and fry.",
        Ingredients: [new IngredientDto("Flour", 150m, "g"), new IngredientDto("Eggs", 2m, null)]);

    [Fact]
    public void ValidRequest_HasNoErrors()
    {
        _validator.TestValidate(ValidRequest()).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Name_IsRequired(string name)
    {
        var result = _validator.TestValidate(ValidRequest() with { Name = name });

        result.ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Fact]
    public void Name_LongerThan200_IsInvalid()
    {
        var result = _validator.TestValidate(ValidRequest() with { Name = new string('a', 201) });

        result.ShouldHaveValidationErrorFor(r => r.Name);
    }

    [Fact]
    public void Description_LongerThan1000_IsInvalid()
    {
        var result = _validator.TestValidate(ValidRequest() with { Description = new string('a', 1001) });

        result.ShouldHaveValidationErrorFor(r => r.Description);
    }

    [Fact]
    public void Description_Null_IsValid()
    {
        var result = _validator.TestValidate(ValidRequest() with { Description = null });

        result.ShouldNotHaveValidationErrorFor(r => r.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Servings_OutsideRange_IsInvalid(int servings)
    {
        var result = _validator.TestValidate(ValidRequest() with { Servings = servings });

        result.ShouldHaveValidationErrorFor(r => r.Servings);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void Servings_AtBoundaries_IsValid(int servings)
    {
        var result = _validator.TestValidate(ValidRequest() with { Servings = servings });

        result.ShouldNotHaveValidationErrorFor(r => r.Servings);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1441)]
    public void PrepMinutes_OutsideRange_IsInvalid(int minutes)
    {
        var result = _validator.TestValidate(ValidRequest() with { PrepMinutes = minutes });

        result.ShouldHaveValidationErrorFor(r => r.PrepMinutes);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1441)]
    public void CookMinutes_OutsideRange_IsInvalid(int minutes)
    {
        var result = _validator.TestValidate(ValidRequest() with { CookMinutes = minutes });

        result.ShouldHaveValidationErrorFor(r => r.CookMinutes);
    }

    [Fact]
    public void ZeroMinutes_IsValid()
    {
        var result = _validator.TestValidate(ValidRequest() with { PrepMinutes = 0, CookMinutes = 0 });

        result.ShouldNotHaveValidationErrorFor(r => r.PrepMinutes);
        result.ShouldNotHaveValidationErrorFor(r => r.CookMinutes);
    }

    [Fact]
    public void Ingredients_Empty_IsInvalid()
    {
        var result = _validator.TestValidate(ValidRequest() with { Ingredients = [] });

        result.ShouldHaveValidationErrorFor(r => r.Ingredients);
    }

    [Fact]
    public void InvalidIngredient_ReportsErrorWithItsIndex()
    {
        var request = ValidRequest() with
        {
            Ingredients = [new IngredientDto("Flour", 150m, "g"), new IngredientDto("", 0m, "g")]
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor("Ingredients[1].Name");
        result.ShouldHaveValidationErrorFor("Ingredients[1].Quantity");
        result.ShouldNotHaveValidationErrorFor("Ingredients[0].Name");
    }

    [Fact]
    public void UpdateValidator_AppliesTheSameRules()
    {
        var validator = new UpdateRecipeRequestValidator();
        var request = new UpdateRecipeRequest("", null, 0, -1, 0, null, []);

        var result = validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Name);
        result.ShouldHaveValidationErrorFor(r => r.Servings);
        result.ShouldHaveValidationErrorFor(r => r.PrepMinutes);
        result.ShouldHaveValidationErrorFor(r => r.Ingredients);
    }
}

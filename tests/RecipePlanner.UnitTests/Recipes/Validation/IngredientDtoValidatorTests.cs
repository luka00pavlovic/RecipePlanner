using FluentValidation.TestHelper;
using RecipePlanner.Application.Recipes;
using RecipePlanner.Application.Recipes.Validation;

namespace RecipePlanner.UnitTests.Recipes.Validation;

public class IngredientDtoValidatorTests
{
    private readonly IngredientDtoValidator _validator = new();

    [Fact]
    public void ValidIngredient_HasNoErrors()
    {
        _validator.TestValidate(new IngredientDto("Flour", 200m, "g")).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void NullUnit_IsValid()
    {
        _validator.TestValidate(new IngredientDto("Eggs", 2m, null)).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Name_IsRequired(string name)
    {
        _validator.TestValidate(new IngredientDto(name, 1m, "g")).ShouldHaveValidationErrorFor(i => i.Name);
    }

    [Fact]
    public void Name_LongerThan100_IsInvalid()
    {
        _validator.TestValidate(new IngredientDto(new string('a', 101), 1m, "g"))
            .ShouldHaveValidationErrorFor(i => i.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Quantity_NotPositive_IsInvalid(decimal quantity)
    {
        _validator.TestValidate(new IngredientDto("Flour", quantity, "g"))
            .ShouldHaveValidationErrorFor(i => i.Quantity);
    }

    [Fact]
    public void Unit_LongerThan20_IsInvalid()
    {
        _validator.TestValidate(new IngredientDto("Flour", 1m, new string('a', 21)))
            .ShouldHaveValidationErrorFor(i => i.Unit);
    }
}

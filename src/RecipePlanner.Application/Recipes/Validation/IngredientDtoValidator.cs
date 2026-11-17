using FluentValidation;

namespace RecipePlanner.Application.Recipes.Validation;

public class IngredientDtoValidator : AbstractValidator<IngredientDto>
{
    public IngredientDtoValidator()
    {
        RuleFor(i => i.Name).NotEmpty().MaximumLength(100);
        RuleFor(i => i.Quantity).GreaterThan(0);
        RuleFor(i => i.Unit).MaximumLength(20);
    }
}
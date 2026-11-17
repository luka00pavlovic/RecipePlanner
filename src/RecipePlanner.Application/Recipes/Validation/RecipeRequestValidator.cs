using FluentValidation;

namespace RecipePlanner.Application.Recipes.Validation;

public abstract class RecipeRequestValidator<T> : AbstractValidator<T> where T : IRecipeRequest
{
    protected RecipeRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(200);
        RuleFor(r => r.Description).MaximumLength(1000);
        RuleFor(r => r.Servings).InclusiveBetween(1, 100);
        RuleFor(r => r.PrepMinutes).InclusiveBetween(0, 1440);
        RuleFor(r => r.CookMinutes).InclusiveBetween(0, 1440);
        RuleFor(r => r.Ingredients).NotEmpty();
        RuleForEach(r => r.Ingredients).SetValidator(new IngredientDtoValidator());
    }
}

public class CreateRecipeRequestValidator : RecipeRequestValidator<CreateRecipeRequest>;
public class UpdateRecipeRequestValidator : RecipeRequestValidator<UpdateRecipeRequest>;
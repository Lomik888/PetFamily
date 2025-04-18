using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.Validations;

public static class CustomValidator
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);

            if (result.IsSuccess)
            {
                return;
            }

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, IEnumerable<Error>>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);

            if (result.IsSuccess)
            {
                return;
            }

            foreach (var error in result.Error)
            {
                context.AddFailure(error.Serialize());
            }
        });
    }
}
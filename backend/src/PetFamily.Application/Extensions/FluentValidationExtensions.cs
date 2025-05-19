using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptionsConditions<T, TProperty> Must<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, UnitResult<Error>> predicate)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = predicate(value);
            
            if (result.IsSuccess)
            {
                return;
            }

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static IRuleBuilderOptions<T, TProperty> WithMessageCustom<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder,
        string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Сообщение ошибки не должно быть пустым.", nameof(errorMessage));

        var error = ErrorsPreform.General
            .Validation(errorMessage, nameof(T))
            .Serialize();

        DefaultValidatorOptions.Configurable(ruleBuilder).Current.SetErrorMessage(error);

        return ruleBuilder;
    }

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
        Func<TElement, Result<TValueObject, ErrorList>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);

            if (result.IsSuccess)
            {
                return;
            }

            foreach (var error in result.Error.Errors)
            {
                context.AddFailure(error.Serialize());
            }
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

    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);

            if (result.IsSuccess)
            {
                return;
            }

            context.AddFailure(result.Error);
        });
    }
}
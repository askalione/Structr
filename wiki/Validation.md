# Validation

**Structr.Validation** package is intended to help organize model (entities, dtos, commands, queries, etc.) validation in application.

## Installation

Validation package is available on [NuGet](https://www.nuget.org/packages/Structr.Validation/).

```
dotnet add package Structr.Validation
```

## Setup

Create a class whose instances you want to validate, for example, `Contract`:

```csharp
public class Contract
{
    public string Number { get; set; };
    public string Title { get; set; };
}
```

Create a validator class that inherits from `Validator<T>` with generic parameter `Contract`, for example, `ContractValidator` and implement a validation logic in method `Validate()`:

```csharp
internal class ContractValidator : Validator<Contract>
    {
        protected override ValidationResult Validate(Contract instance)
        {
            var failures = new List<ValidationFailure>();

            if (instance.Title.Length > 10)
            {
                failures.Add(new ValidationFailure(nameof(instance.Title), instance.Title, "Title is too long."));
            }

            if (instance.Number.Length != 5)
            {
                failures.Add(new ValidationFailure(nameof(instance.Number), instance.Number, "Document number should be 5 characters."));
            }

            /* Another validation */

            return failures.ToValidationResult();
        }
    }
```

And then setup validation services:

```csharp
services.AddValidation(typeof(ContractValidator).Assembly);
```

## Usage

After setup you can use `IValidationProvider` in your application that allows you to validate instances asynchronously with the method `ValidateAndThrowAsync()`, like this:

```csharp
    public class ContractService
    {
        private readonly IValidationProvider _validationProvider;

        public ContractService(IValidationProvider validationProvider)
        {
            _validationProvider = validationProvider;
        }

        public async Task EditAsync(ContractEditCommand command, CancellationToken cancellationToken)
        {
            /* Get contract with Id == id */
            
            contract.Number = command.Number;
            contract.Title = command.Title;
            
            await _validationProvider.ValidateAndThrowAsync(contract, cancellationToken);
        }
    }
```

You can also use the method `ValidateAsync()` that returns a list of validation failures that are wrapped in `ValidationResult`:

```csharp
ValidationResult result = await _validationProvider.ValidateAsync(contract);
```

`ValidationResult` is a collection of `ValidationFailure`.

`ValidationFailure` has the following fields:

| Field | Type | Default value | Comment |
| --- | --- | --- | --- |
| Message | string | - | The message that describes the failure. |
| ParameterName | string | null | The name of the parameter that caused the failure. |
| ActualValue | object | null | The value of the parameter that caused the failure. |
| Code | string | null | The failure code. |
| Level | ValidationFailureLevel | Error | The level of the failure. |

## Extensions

You can implement your custom validation provider `CustomValidationProvider` that inherits from `IValidationProvider` and use it at setup, for example:

```csharp
services.AddValidation(options =>
  {
      options.ProviderType = typeof(CustomValidationProvider);
  },
  typeof(ContractValidator).Assembly);
```

You can override the lifetime of the validation provider, for example:

```csharp
services.AddValidation(options =>
  {
      options.Lifetime = ServiceLifetime.Transient;
  },
  typeof(ContractValidator).Assembly)
```
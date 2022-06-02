# Validation

**Structr.Validation** package provides functionality for building strongly-typed validation.

## Installation

Validation package is available on [NuGet](https://www.nuget.org/packages/Structr.Validation/).

```
dotnet add package Structr.Validation
```

## Setup

Configure validation services:

```csharp
services.AddValidation(typeof(Program).Assembly);
```

`AddValidation()` extension method performs registration of validation provider service and validators implementing `IValidator` or inherited from `Validator` class.

| Parameter name | Parameter type | Description |
| --- | --- | --- |
| assembliesToScan | `params Assembly[]` | List of assemblies to search validators. |
| configureOptions | `Action<ValidationServiceOptions>` | Options to be used by validation service. | 

Additionally configure `IValidationProvider` service by specifying it's type and lifetime used `ValidationServiceOptions`.

| Property name | Property type | Description |
| --- | --- | --- |
| ProviderType | `Type` | Changes standard implementation of `IValidationProvider` to specified one, default is `typeof(ValidationProvider)`. | 
| Lifetime | `ServiceLifetime` | Specifies the lifetime of an `IValidationProvider` service, default is `Scoped`. |

## Usage

Validation objects can be Entities, DTO's, Commands, Queries and many others typed objects.

The basic usage is:

```csharp
public record UserCreateCommand : IOperation<int>
{
    public string Email { get; init; }
    public string Password { get; init; }
}
public class UserCreateCommandValidator : IValidator<UserCreateCommand>
{
    private readonly IDbContext _dbContext;

    public UserCreateCommandValidator(IDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<ValidationResult> ValidateAsync(UserCreateCommand command, CancellationToken cancellationToken)
    {
        var failures = List<ValidationFailure>();

        if (string.IsNullOrWhitespace(command.Email))
        {
            failures.Add(new ValidationFailure(nameof(command.Email), command.Email, "Email is required."));
        }
        if (IsValidEmail(command.Email) == false)
        {
            failures.Add(new ValidationFailure(nameof(command.Email), command.Email, "Email is invalid."));
        }
        bool isUniqueEmail = await IsUniqueEmailAsync(command.Email, cancellationToken);
        if (isUniqueEmail == false)
        {
            failures.Add(new ValidationFailure(nameof(command.Email), command.Email, "Email is not unique."));
        }
        if (string.IsNullOrWhitespace(command.Password))
        {
            failures.Add(new ValidationFailure(nameof(command.Password), command.Password, "Password is required."));
        }
        if (IsValidPassword(command.Email) == false)
        {
            failures.Add(new ValidationFailure(nameof(command.Password), command.Password, "Password is invalid."));
        }

        return failures.ToValidationResult();
    }

    private bool IsValidEmail(string email)
    {
        /* Some logic here*/
    }

    private async Task<bool> IsUniqueEmailAsync(string email, CancellationToken cancellationToken)
        => await _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken) == false;

    private bool IsValidPassword(string password)
    {
        /* Some logic here*/
    }
}
```

The last step is to inject `IValidationProvider` service and use it:

```csharp
public class UserCreateCommandHandler : IOperationHandler<UserCreateCommand, int>
{
    private readonly IDbContext _dbContext;
    private readonly IValidationProvider _validationProvider;

    public UserCreateCommandHandler(IDbContext dbContext, IValidationProvider validationProvider)
    {
        _dbContext = dbContext;
        _validationProvider = validationProvider;
    }

    public async Task<Guid> HandleAsync(UserCreateCommand command, CancellationToken cancellationToken)
    {
        // Validate incoming command and throw exception if failures.
        await _validationProvider.ValidateAndThrowAsync(command);

        /* Some logic here */
    }
}
```

**Recommendation**: For validating Commands/Queries use `IOperationFilter` instead of calling `_validationProvider.ValidateAndThrowAsync()` in each command/query.
See more details about [Operations](Operations/Operations.md).

`IValidationProvider` methods:

| Method name | Return type | Description |
| --- | --- | --- |
| ValidateAsync | `ValidationResult` | Asynchronously validates the object and returns the `ValidationResult`. |
| ValidateAndThrowAsync | - | Asynchronously validates the object and throws `ValidationException` if validation result has failures. |

`ValidationFailure` represents a single validation error.

`ValidationFailure` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| ParameterName | `string` | The name of the parameter that caused the failure. |
| ActualValue | `object` | The value of the parameter that caused the failure. |
| Message | `string` | The message that describes the failure. |
| Code | `string` | The optional failure code. |
| Level | `ValidationFailureLevel` | The optional level of failure - `Error`, `Warning` or `Info`. Default value is `Error`. |

`ValidationResult` represents all failures that occur during validation execution.

`ValidationResult` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| IsValid | `bool` | Returns `true` if the validation result has not failures, otherwise `false`. |

`ValidationResult` class inherits from `IEnumerable<ValidationFailure>` and allow you to use iteration:

```csharp
ValidationResult validationResult = await _validationProvider.ValidateAsync(command);
if (validationResult.IsValid == false)
{
    foreach(ValidationFailure validationFailure in validationResult)
    {
        _logger.LogError(validationFailure.Message);
    }
}
```

Thrown `ValidationException` class has following main properties:

| Property name | Property type | Description |
| --- | --- | --- |
| ValidationResult | `ValidationResult` | The result of validation. |
| Message | `string` | The messages of all failures joined into one string. |

```csharp
try
{
    await _validationProvider.ValidateAsync(command);
}
catch(ValidationException ex)
{
    // Option 1 - Use default exception message property:
    _logger.LogError(ex.Message);

    // Option 2 - Iterate all failures with message:
    foreach(ValidationFailure validationFailure in ex.ValidationResult)
    {
        _logger.LogError(validationFailure.Message);
    }
}
```
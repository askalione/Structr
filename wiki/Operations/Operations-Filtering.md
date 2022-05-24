# Filtering

**Structr.Operations** allows you to introduce some filtering in operation processing pipeline. This could be helpful if some verification or logging is needed.
Filtering setup is rather simple, just register your filter as a service and everything is done.

Filter implies implementing `IOperationFilter<TOperation, TResult>` interface and registering corresponding service in DI container:

```csharp
services.AddTransient(typeof(IOperationFilter<,>), typeof(CommandValidationFilter<,>));
services.AddTransient(typeof(IOperationFilter<,>), typeof(QueryLoggingFilter<,>));
```

The filter interface described below:

```csharp
public interface IOperationFilter<in TOperation, TResult> where TOperation : IOperation<TResult>
{
    Task<TResult> FilterAsync(TOperation operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next);
}
```

## Filter conditional execution

Filter gets operation-object itself, cancellation token and supplied with delegate representing next filter to be applied or operation handler itself. So it should be called in some place in code to not to brake processing chain. Or if you need it could be made conditional. Example of such filter is provided below:

```csharp
public class CommandValidationFilter<TCommand, TResult> : IOperationFilter<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private readonly IValidationProvider _validationProvider;

    public CommandValidationFilter(IValidationProvider validationProvider) 
        => _validationProvider = validationProvider;

    public async Task<TResult> FilterAsync(TCommand command, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
    {
        if (_validationProvider.IsValid(command, cancellationToken))
        {
            return await next();
        }
        else
        {
            throw new SomeException("Some error message.");
        }
    }
}
```

With filtering pipeline u could implement you custom post- and pre- processors for any operations you need.

## Special filters

As you can see in example before we've used some `ICommand<TResult>` interface. This could be a suitable solution to separate your data-changing and not-data-changing operations i.e. commands and queries respectively. Suggested interfaces you could create in your App are:

```csharp
public interface IQuery<TResult> : IOperation<TResult>
{ }

public interface ICommand<TResult> : IOperation<TResult>
{ }

public interface ICommand : ICommand<VoidResult>, IOperation
{ }
```

Using them you can create filters separately for queries and separately for commands:

```csharp
// This one only for commands.
public class CommandValidationFilter<TCommand, TResult> : IOperationFilter<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{}

// This is for queries.
public class QueryLoggingFilter<TQuery, TResult> : IOperationFilter<TQuery, TResult> 
    where TQuery : IQuery<TResult>
{}

// And this one would work for both operations types.
public class UniversalFilter<TOperation, TResult> : IOperationFilter<TOperation, TResult> 
    where TOperation : IOperation<TResult>
{}
```
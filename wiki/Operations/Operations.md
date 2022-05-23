# Structr.Operations

Simple yet fully functional commands and queries executor for .NET. It implements CQRS pattern which allows you to decouple read and update operations for a data store.

Operations library is cross-platform, has no external dependencies and uses `netstandard2.0` as a base. This allows you to use it in both legacy .NET Framework and any new .NET Core 2.0+ projects.

## Installation

Operations package is available on [NuGet](https://www.nuget.org/packages/Structr.Operations/). 

Use command line

```
dotnet add package Structr.Operations
```

or Package-Manager console command

```
Install-Package Structr.Operations
```

to install this library.

## Setup and basic usage

Basic setup is pretty simple and requires only adding registration of `IOperationExecutor` service to your DI-container. Corresponding example for Microsoft DI - `Microsoft.Extensions.DependencyInjection.ServiceCollection` is provided below:

```csharp
var services = new ServiceCollection();
services.AddOperations(typeof(Program).Assembly);
```

Here the only parameter of [`AddOperations`](/Operations-Extensions.md) extension method gives a place to search and register yours implementations of commands and queries processors. Sample query handler with query object are:

```csharp
public class FooQuery : IOperation<int>
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
}
public class FooQueryHandler : AsyncOperationHandler<FooQuery, int>
{
    private readonly IStringWriter _writer;
    public FooQueryHandler(IStringWriter writer) => _writer = writer;
    public override async Task<int> HandleAsync(FooQuery operation, CancellationToken cancellationToken)
    {
        var sum = operation.Number1 + operation.Number2;
        var result = $"Handle FooQuery. Sum is `{sum}`";
        await _writer.WriteLineAsync(result);
        return sum;
    }
}
```

The last step is to inject IOperationExecutor into your class and use it:

```csharp
class MyWorker
{
    private readonly IOperationExecutor _executor;
    public MyWorker(IOperationExecutor executor) => _executor = executor;
    public async Task DoWork()
    {
        var myQuery = new FooQuery { Number1 = 3, Number2 = 4 };
        var result = await _executor.ExecuteAsync(myQuery); // this will give 7
    }
}
```

That's it! Simple use is really simple but there are more cool stuff which can be done with `Structr.Operations` such as: operations filtering and handlers decoration. Details will be describer further.

## Operations

Structr.Operation uses term 'operation' and corresponding interface `IOperation` to describe an object containing data needed to handle operation. As in classical CQRS operations could be divided into Queries and Commands, former of which intended to be used in operations that don't change any data in storages or objects states and the latter one vice versa.

Depending on if your operation is intended to return some result or not, it should implement one of the following interfaces: `IOperation<out TResult>` or `IOperation`. For example processing of such operation (query) should return `int` value:

```csharp
public class SumQuery : IOperation<int>
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
}
```

And no value for such operation (command):

```csharp
public class DropAllDataBaseCommand : IOperation
{
    public bool AreYouSure { get; set; }
}
```

## Operation execution and handlers

Operations performing happens in OperationHandlers - separate classes that are inherited from one of the following base classes:

* `AsyncOperationHandler<TOperation, TResult>` - async and value-returning handler;
* `AsyncOperationHandler<TOperation>` - async and nothing-returning handler;
* `OperationHandler<TOperation, TResult>` - synchronic and value-returning handler;
* `OperationHandler<TOperation>` - synchronic and nothing-returning handler;

The example for simple command and it's handler is:

```csharp
public class EditUserCommand : ICommand
{
    public int UserId { get; set; }
    public string Name { get; set; }
}
public class EditUserCommandHandler : AsyncOperationHandler<EditUserCommand>
    {
        private readonly DataContext _dataContext;
        public EditUserCommandHandler(IStringWriter dataContext)
        {
            _dataContext = dataContext;
        }
        public override async Task HandleAsync(EditUserCommand operation, CancellationToken cancellationToken)
        {
            /*some database work here*/
        }
    }
```

Thou you could implement `IOperationHandler` interface directly but it will create more handwork for you when writing additional `return VoidResult.Value` statement to imitate no returning result in corresponding situations. So better to inherit one of four classes described earlier.

## Filtering

Structr.Operations allows you to introduce some filtering in operation processing pipeline. This could be helpful if some verification or logging is needed.
Filtering setup is rather simple, just register your filter as a service and everything is done:

```csharp
services.AddTransient(typeof(IOperationFilter<,>), typeof(OperationLoggingFilter<,>));
```

Here we've used build-in interface `IOperationFilter<,>` that is intended to represent custom filter methods to be applied in overall processing pipeline. The sample implementation of a filter is presented below:

```csharp
public class OperationLoggingFilter<TOperation, TResult> : IOperationFilter<TOperation, TResult> where TOperation : IOperation<TResult>
{
    private readonly IStringWriter _writer;
    public OperationLoggingFilter(IStringWriter writer)
    {
        _writer = writer;
    }
    public async Task<TResult> FilterAsync(TQuery operation, CancellationToken cancellationToken, OperationHandlerDelegate<TResult> next)
    {
        await _writer.WriteLineAsync($"Operation logging: Call operation of type {operation.GetType().Name}");
        return await next();
    }
}
```

All filters are applying into processing pipeline in same order they were registered in service collections, so this is [controllable](/Operations-Filtering.md#filter-conditional-execution). `await next()` statement performs calling of next action in whole pipeline til the handler itself, so some actions can be performed already after operation handling just by swaping `next()` and filter code itself.
Filters are also could be assigned separately for queries and commands. Details are [here](/Operations-Filtering.md#special-filters).

## Operations decoration

Using abilities of Microsoft's (and not only theirs) DI-container you could decorate your operations handlers with additional logic which will be applied in processing pipeline. This really almost the same functionality as with **Filters** but in some cases it could have it's own place. He is an example of such feature:

```csharp
public class CommandDecorator<TCommand, TResult> : BaseOperationDecorator<TCommand, TResult>, ICommandDecorator<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private readonly IStringWriter _writer;
    public CommandDecorator(IStringWriter writer) => _writer = writer;
    public override async Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken)
    {
        await _writer.WriteLineAsync($"Preprocess command `{typeof(TCommand).Name}` by `{GetType().Name}`");
        var result = await handler.HandleAsync(command, cancellationToken);
        await _writer.WriteLineAsync($"Postprocess command `{typeof(TCommand).Name}` by `{GetType().Name}`");
        return result;
    }
}
```

ICommand is [described here](/Operations-Filtering.md#special-filters).
Now we must register it as service in our DI-container with it's base `OperationDecorator<,>`:

```csharp
services.AddTransient(typeof(ICommandDecorator<,>), typeof(CommandDecorator<,>));
services.Decorate(typeof(IOperationHandler<,>), typeof(OperationDecorator<,>));
```

and then just run an execution of our query:

```csharp
var result = await _executor.ExecuteAsync(myQuery);
```

Along with handler itself the decorator will be processed too. More detailed description of this approach could be [found here](/Operations-Decoration.md).
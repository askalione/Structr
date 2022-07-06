# Operations

**Structr.Operations** - is simple yet fully functional in-process messaging library, that allows you to easy implement CQS/CQRS.

## Installation

Operations package is available on [NuGet](https://www.nuget.org/packages/Structr.Operations/). 

```
dotnet add package Structr.Operations
```

## Setup

Basic setup is pretty simple and requires only adding registration of `IOperationExecutor` service to your DI-container:

```csharp
services.AddOperations(typeof(Program).Assembly);
```

`AddOperations()` extension method performs registration of executor service and operation handlers implementing `IOperationHandler` or inherited from `OperationHandler` and `AsyncOperationHandler` classes.

| Parameter name | type | Description |
| --- | --- | --- |
| assembliesToScan | `params Assembly[]` | List of assemblies to search operation handlers. |
| configureOptions | `Action<OperationServiceOptions>` | Options to be used by operations handling service. | 

Additionally configure `IOperationExecutor` service by specifying it's type and lifetime used `OperationServiceOptions`.

| Property name | Property type | Description |
| --- | --- | --- |
| ExecutorServiceType | `Type` | Changes standard implementation of `IOperationExecutor` to specified one, default is `typeof(OperationExecutor)`. | 
| ExecutorServiceLifetime | `ServiceLifetime` | Specifies the lifetime of an `IOperationExecutor` service, default is `Transient`. |

## Usage

The basic usage is:

```csharp
public class FooQuery : IOperation<int>
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
}
public class FooQueryHandler : AsyncOperationHandler<FooQuery, int>
{
    private readonly IStringWriter _writer;

    public FooQueryHandler(IStringWriter writer)
        => _writer = writer;

    public override async Task<int> HandleAsync(FooQuery query, CancellationToken cancellationToken)
    {
        var sum = query.Number1 + query.Number2;
        var result = $"Handle FooQuery. Sum is `{sum}`";
        await _writer.WriteLineAsync(result);
        return sum;
    }
}
```

The last step is to inject `IOperationExecutor` service and use it:

```csharp
public class FooController : Controller
{
    private readonly IOperationExecutor _executor;

    public FooController(IOperationExecutor executor) 
        => _executor = executor;

    public async Task<IActionResult> Index()
    {
        var myQuery = new FooQuery { Number1 = 3, Number2 = 4 };
        var result = await _executor.ExecuteAsync(myQuery); // This will give 7.
    }
}
```

That's it! Simple use is really simple but there are more cool stuff which can be done with `Structr.Operations` such as: [operations filtering](Filtering.md) and [handlers decoration](Decoration.md).

### Operations

Structr.Operation uses term "operation" and corresponding interface `IOperation` to describe an object containing data needed to handle operation. 

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

### Operation handlers

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
            => _dataContext = dataContext;

        public override async Task HandleAsync(EditUserCommand operation, CancellationToken cancellationToken)
        {
            /* Some database work here. */
        }
    }
```

Thou you could implement `IOperationHandler` interface directly but it will create more handwork for you when writing additional `return VoidResult.Value` statement to imitate no returning result in corresponding situations. So better to inherit one of four classes described earlier.
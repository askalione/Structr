# Decoration

Using abilities of Microsoft's (and not only theirs) DI-container you could decorate your operations handlers with additional logic which will be applied in processing pipeline. This really almost the same functionality as with [Filters](Filtering.md) but in some cases it could have it's own place. In example below we will add some decorators to process our [queries and commands](Filtering.md#special-filters).

```csharp
// Define interface for all decorators.
public interface ICommandDecorator<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken);
}
public interface IQueryDecorator<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> DecorateAsync(TQuery query, IOperationHandler<TQuery, TResult> handler, CancellationToken cancellationToken);
}
public interface ICommandDecorator<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> DecorateAsync(TCommand command, IOperationHandler<TCommand, TResult> handler, CancellationToken cancellationToken);
}

// Define base decorator for all your decorators
public abstract class BaseOperationDecorator<TOperation, TResult> where TOperation : IOperation<TResult>
{
    public abstract Task<TResult> DecorateAsync(TOperation operation, IOperationHandler<TOperation, TResult> handler, CancellationToken cancellationToken);
}

// Define operation decorator that will dispatch command and query decorators
public class OperationDecorator<TOperation, TResult> : IOperationHandler<TOperation, TResult>
    where TOperation : IOperation<TResult>
{
    private readonly IOperationHandler<TOperation, TResult> _handler;
    private readonly IServiceProvider _serviceProvider;

    public OperationDecorator(IOperationHandler<TOperation, TResult> handler,
        IServiceProvider serviceProvider)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }
        if (serviceProvider == null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }
        _handler = handler;
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken)
    {
        if (operation is ICommand<TResult>)
        {
            return ((BaseOperationDecorator<TOperation, TResult>)_serviceProvider.GetService(typeof(ICommandDecorator<,>).MakeGenericType(operation.GetType(), typeo(TResult))))
                .DecorateAsync(operation, _handler, cancellationToken);
        }
        else if (operation is IQuery<TResult>)
        {
            return ((BaseOperationDecorator<TOperation, TResult>)_serviceProvider.GetService(typeof(IQueryDecorator<,>).MakeGenericType(operation.GetType(), typeof(TResult))))
                .DecorateAsync(operation, _handler, cancellationToken);
        }
        else
        {
            return _handler.HandleAsync(operation, cancellationToken);
        }
    }
}

// Create your own specific command decorator that have some business-logic payload.
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

That's it! Now running of every command will be accompanied by writing some logs.
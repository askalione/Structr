# ServiceCollectionExtensions

To simplify process of registration of executor service and operation handlers IServiceCollections extension methods are introduced. They give you an opportunity to specify assemblies to search for handlers and some options to configure executor service.

## AddOperations

Performs registration of executor service and operation handles implementing IOperationHandler or inherited from OperationHandler and AsyncOperationHandler classes.

| Parameter name | type | Description |
| --- | --- | --- |
| configureOptions | `Action<OperationServiceOptions>` | Options to be used by operations handling service. | 
| assembliesToScan | params `Assembly[]` | List of assemblies to search operation handlers. | 

## OperationServiceOptions

This one could be used to additionally configure `IOperationExecutor` service by specifying it's type and lifetime.

| Property name | Property type | Description |
| --- | --- | --- |
| ExecutorType | `Type` | Changes standard implementation of `IOperationExecutor` to specified one. | 
| ExecutorServiceLifetime | Specifies the lifetime of an `IOperationExecutor` service. | 
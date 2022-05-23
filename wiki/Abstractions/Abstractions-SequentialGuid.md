# SequentialGuid

`SequentialGuid` class provides tools for server side generation sequential GUID.

## Methods

| Method name | Return type | Description |
| --- | --- | --- |
| NewGuid | `Guid` | Generate new sequential GUID. | 

```csharp
var fileId = SequentialGuid.NewGuid();
```

Example of sequential guids:

```
cc6466f7-1066-11dd-acb6-005056c00008
cc6466f8-1066-11dd-acb6-005056c00008
cc6466f9-1066-11dd-acb6-005056c00008
```
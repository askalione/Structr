# TimestampProvider

TimestampProvider provides functionality to generating timestamp value.

## Setup

You can create custom timestamp provider:

```csharp
public class CustomTimestampProvider : ITimestampProvider
{
    public DateTime GetTimestamp()
    {
        return DateTime.UtcNow;
    }
}
```

And then setup services:

```csharp
services.AddTimestampProvider<CustomTimestampProvider>();
```
Or you can use one of default implemented configuration provider from list:

* `LocalTimestampProvider` - simply returns DateTime.Now;
* `UtcTimestampProvider` - returns DateTime.UtcNow.

## Usage

Inject `ITimestampProvider` service and use it:

```csharp
public class FileService : ICustomService
{
    private readonly ITimestampProvider _timestampProvider;

    public FileService(ITimestampProvider timestampProvider)
        => _timestampProvider = timestampProvider;

    public void Upload()
    {
        DateTime fileUploadedAt = _timestampProvider.GetTimestamp();
    }
}
```

`ITimestampProvider` methods list:

| Method name | Return type | Description |
| --- | --- | --- |
| GetTimestamp | `DateTime` | Generate new timestamp. |


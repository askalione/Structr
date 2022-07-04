# SequentialGuidProvider

`SequentialGuidProvider` provides functionality for server side generation sequential GUID (COMB GUID).

## Setup

The basic setup is:

```csharp
services.AddSequentialGuidProvider();
```

`AddSequentialGuidProvider` extension method performs registration of SequentialGuidProvider with specified settings.

| Parameter name | Parameter type | Description |
| --- | --- | --- |
| initializer | `SequentialGuidInitializer` | Initializer which returns GUID to combine with timestamp. Default value is `Guid.NewGuid`. |
| timestampProvider | `SequentialGuidTimestampProvider` | Timestamp provider for generating COMB GUID. |

## Usage

Inject `ISequentialGuidProvider` service and use it:

```csharp
public class FileService : ICustomService
{
    private readonly ISequentialGuidProvider _sequentialGuidProvider;

    public FileService(ISequentialGuidProvider _sequentialGuidProvider)
        => _sequentialGuidProvider = _sequentialGuidProvider;

    public void Upload()
    {
        Guid fileId = _sequentialGuidProvider.GetSequentialGuid(SequentialGuid.String);
    }
}
```

`ITimestampProvider` methods list:

| Method name | Return type | Description |
| --- | --- | --- |
| GetSequentialGuid | `Guid` | Generate new sequential GUID. |

`GetSequentialGuid` method generating sequential GUID with following types:

* [String](#string)
* [Binary](#binary)
* [Ending](#ending)

### String

`SequentialGuidType.String` - The first six bytes are in sequential order, and the remainder is random. Inserting these values into a database that stores GUIDs as strings (such as MySQL) should provide a performance gain over non-sequential values. This type should be used with **MySQL** or **PostgreSQL** database.


### Binary

`SequentialGuidType.Binary` - The first two blocks are "jumbled" due to having all their bytes reversed (this is due to the endianness issue discussed earlier). If we were to insert these values into a text field (like they would be under MySQL or PostgreSQL), the performance would not be ideal. This type should be used with **Oracle** database.

### Ending

`SequentialGuidType.Ending` - The last six bytes are in sequential order, and the rest is random. This type should be used with **MS SQL Server** database.
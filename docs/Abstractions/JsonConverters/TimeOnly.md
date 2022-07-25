# TimeOnlyJsonConverter

`TimeOnlyJsonConverter` provides functionality for converting a `TimeOnly` to or from JSON.

## Usage

For example some serializable class:

```csharp
class Timer
{
    public TimeOnly StartFrom { get; set; }
}
```
Deserialize:

```csharp
var options = new JsonSerializerOptions();
options.Converters.Add(new TimeOnlyJsonConverter());

string json = "{ \"StartFrom\": \"11:15:30.000\" }";
Timer timer = JsonSerializer.Deserialize<Timer>(json, options);
TimeOnly startFrom = timer.StartFrom; // ---> 11:15:30.000
```
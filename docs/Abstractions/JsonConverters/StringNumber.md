# StringNumberJsonConverter

`StringNumberJsonConverter` provides functionality for converting a `int` from JSON to `string`.

## Usage

For example some serializable class:

```csharp
class Order
{
    public string Id { get; set; }
}
```
Deserialize:

```csharp
var options = new JsonSerializerOptions();
options.Converters.Add(new StringNumberJsonConverter());

string json = "{ \"Id\": 72346 }";
Order order = JsonSerializer.Deserialize<Order>(json, options);
string id = order.Id; // ---> 72346
```
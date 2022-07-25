# DateOnlyJsonConverter

`DateOnlyJsonConverter` provides functionality for converting a `DateOnly` to or from JSON.

## Usage

For example some serializable class:

```csharp
class Person
{
    public DateOnly DateOfBirth { get; set; }
}
```
Deserialize:

```csharp
var options = new JsonSerializerOptions();
options.Converters.Add(new DateOnlyJsonConverter());

string json = "{ \"DateOfBirth\": \"1903-05-01\" }";
Person person = JsonSerializer.Deserialize<Person>(json, options);
DateOnly dateOfBirth = person.DateOfBirth; // ---> 1903-05-01
```
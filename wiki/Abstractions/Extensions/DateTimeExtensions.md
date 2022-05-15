## ToShortDateString

Converts the value of the current `DateTime?` object to its equivalent short date string representation.
If value is null then returns specified `defaultValue`

```csharp
DateTime? dateTime = null;
var result = dateTime.ToShortDateString("------"); // "------"

dateTime = new DateTime(2008, 09, 25, 11, 35, 52);
result = dateTime.ToShortDateString("------"); // "09/25/2008"
```

Same works for long DateTime representation - `ToLongDateString`

## ToString

Converts the value of the current `DateTime?` object to its equivalent string representation using the specified format and the formatting conventions of the current culture.
If value is null then returns specified `defaultValue`

```csharp
DateTime? dateTime = null;
var result = dateTime.ToString("------"); // "------"

dateTime = new DateTime(2008, 09, 25, 11, 35, 52);
result = dateTime.ToString("dd-MM-yyyy hh:mm:ss", "------"); // "25-09-2008 11:35:52"
```
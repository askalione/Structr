# Check

`Check` static class with its methods provides functionality for determining if some variable's value meets specified conditions.

## IsInRange

Determines whether specified string value length lays in provided boundaries with inclusion:

```csharp
Check.IsInRange("123", 4, 8); // --> false
Check.IsInRange("123456", 4, 8); // --> true
```

Same method for DateTime determines whether specified value lays in provided boundaries with inclusion:

```csharp
Check.IsInRange("1980-01-01", DateTime.Parse("1990-01-01"), DateTime.Parse("2022-12-31")) // --> false
Check.IsInRange("2000-05-30", DateTime.Parse("1990-01-01"), DateTime.Parse("2022-12-31")) // --> true
```

There are similar methods for checking `IsGreaterThan` and `IsLessThan` conditions.

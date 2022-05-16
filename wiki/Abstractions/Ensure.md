# Ensure
`Ensure` static class with its methods provides functionality for determining if variable's value meets specified conditions and throw exception if it's not.

## NotNull
Throws `ArgumentNullException` when value of specified varible is null:

```csharp
object myValue = null;
Ensure.NotNull(myValue, nameof(myValue)); // --> throws ArgumentNullException
myValue = "123456";
Ensure.NotNull(myValue, nameof(myValue)); // --> does nothing
```

## NotEmpty
Throws ArgumentNullException when value of specified string varible is null or empty:

```csharp
string? myValue = null;
Ensure.NotNull(myValue, nameof(myValue)); // --> throws ArgumentNullException
myValue = "        ";
Ensure.NotNull(myValue, nameof(myValue)); // --> throws ArgumentNullException
myValue = "abcdef";
Ensure.NotNull(myValue, nameof(myValue)); // --> does nothing
```

## Other methods

Other methods to check variables are listed below:

* InRange
* GreaterThan
* LessThan

Each of them got three different versions for `string`, `DateTime` and `IComparable` values.
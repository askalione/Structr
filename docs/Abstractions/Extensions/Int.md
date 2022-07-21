# Int expressions 

## ToKiloFormatString

Creates a human readable kilo format string from `int` value.

```csharp
26.ToKiloFormatString(); // ---> 26
1325.ToKiloFormatString(); // ---> 1,3K
175223.ToKiloFormatString(); // ---> 175K
520138193.ToKiloFormatString(); // ---> 520M
```

## ToPluralFormString

Creates a human readable plural form from `int` value.

```csharp
13.ToPluralFormString(oneForm: "apple", twoForm: "apples", manyForm: "apples"); // ---> apples
```
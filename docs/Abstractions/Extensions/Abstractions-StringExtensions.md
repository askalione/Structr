# String extensions

## DefaultIfEmpty

Return default value if current string is empty, equals null or equals white space, otherwise current string.

```csharp
var result = "      ".DefaultIfEmpty("defaultValue"); // defaultValue
var result = "someValue".DefaultIfEmpty("defaultValue"); // someValue
```

## Cast

Casts string to specified type or throws if cast is impossible and `throwIfInvalidCast` set to `true`.

```csharp
var result = "123,45".Cast(typeof(float?), true); // 123.45F
var result = "123,45abc".Cast(typeof(float?), true); // throws InvalidCastException
var result = "123,45abc".Cast(typeof(float?), false); // null

private enum FooBarBaz
{
    Foo,
    Bar,
    Baz
}
var result = "Bar".Cast(typeof(FooBarBaz?), false); // FooBarBaz.Bar
```

## ToHyphenCase

Formats string value into hyphen case format. For example "ToHyphenCase" will be formated into "to-hyphen-case".

```csharp
var result = "ToHyphenCase".ToHyphenCase() // to-hyphen-case
```

## ToCamelCase

Formats string value into camel case format. For example "ToCamelCase" will be formated into "toCamelCase".

```csharp
var result = "ToCamelCase".ToCamelCase() // toCamelCase
```

## Contains

Returns a value indicating whether a specified string occurs within this string, using the specified comparison rules.
Method could be useful to projects prior to .net standard 2.1. Similar method added to `string` in .net standard 2.1.

```csharp
var result = "QwErTy".Contains("wert", StringComparison.Ordinal) // false
var result = "QwErTy".Contains("wert", StringComparison.OrdinalIgnoreCase) // true
```

## Replace

Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string using provided comparison rule.
Method could be useful to projects prior to .net standard 2.1. Similar method added to `string` in .net standard 2.1.

```csharp
var result = "QwErTyQwErTy".Replace("WeR", "123", StringComparison.Ordinal) // QwErTyQwErTy
var result = "QwErTyQwErTy".Replace("WeR", "123", StringComparison.OrdinalIgnoreCase) // Q123TyQ123Ty
```
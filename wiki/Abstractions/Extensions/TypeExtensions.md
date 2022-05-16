## HasOwnProperty

Gets a value indicating whether public non-static property with a specified name occurs directly within this type.

```csharp
private class Foo
{
    public Bar BarProperty { get; set; }
    public int BarField;
}
public class Bar
{
    public int BarId { get; set; }
}

typeof(Foo).HasOwnProperty("BarProperty") // ---> true
typeof(Foo).HasOwnProperty("BarProperty.BarId") // ---> false
```

## HasNestedProperty

Gets a value indicating whether property with a specified name occurs within this type or its "nested" types.

```csharp
typeof(Foo).HasNestedProperty("BarProperty.BarId") // ---> true
```

## IsNullableEnum

Gets a value indicating whether source type is nullable enumeration.

```csharp
private enum FooBar
{
    Foo,
    Bar
}
var result = typeof(FooBar).IsNullableEnum(); // ---> false
var result = typeof(FooBar?).IsNullableEnum(); // ---> true
```

## GetPropertyInfo

Returns `PropertyInfo` by full property name or `null` if no property was found.

```csharp
typeof(Foo).GetPropertyInfo("BarProperty.BarId"); // ---> PropertyInfo instance for BarId
```

## IsAssignableFromGenericType

Determines whether an instance of a specified type can be assigned to an instance of the current type taking into account generic nature of specified type.

```csharp
private interface IFooGeneric1<T1, T2> { }
private class FooGeneric2<T> : IFooGeneric1<T, DateTime> { }

var result = typeof(IFooGeneric1<,>).IsAssignableFromGenericType(typeof(FooGeneric2<>)); // ---> true
```
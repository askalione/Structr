# MemberInfo extensions

## GetMemberValue

Gets value of object member.

```csharp
private class Foo
{
    public int BarProperty { get; set; }
    public string BarField;
}
var foo = new Foo { BarProperty = 1, BarField = "2" };
var result = typeof(Foo).GetMember("BarField").Single().GetMemberValue(foo); // ---> "2"
```

## GetMemberType

Gets type of object member.

```csharp
var result = typeof(Foo).GetMember("BarProperty").Single().GetMemberType(); // ---> int
```
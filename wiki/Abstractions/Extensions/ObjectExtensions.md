## SetProperty

Sets property of source instance to specified value. When property with provided name doesn't exist then nothing happends.

```csharp
var foo = new Foo { BarProperty = new Bar() };
foo.SetProperty("BarProperty.BarId", 9); // sets value of nested property BarId to 9
```

## Dump

Dumps all object's properties into string for specified count of levels depth.

```csharp
var foo = new Foo
{
    Id = 1,
    BarProperty = new Bar
    {
        BarId = 2,
        BazProperty = new Baz
        {
            BazId = 3,
            BazName = "SomeBaz"
        }
    },
    Flag = true
};
var result = foo.Dump(3);

// results in:
// {Structr.Tests.Abstractions.Extensions.ObjectExtensionsTests+Foo(HashCode:31071611)}
//   Id: 1
//   BarProperty: { }
//     {Structr.Tests.Abstractions.Extensions.ObjectExtensionsTests+Bar(HashCode:63566392)}
//       BarId: 2
//       BazProperty: { }
//   Flag: True
```
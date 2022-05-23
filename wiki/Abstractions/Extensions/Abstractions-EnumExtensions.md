# Enum extensions

## GetDescription

Gets value of the first `DescriptionAttribute` for the enum element.

```csharp
private enum FooBarBaz
{
    [Description("Some Foo enum")]
    Foo,
    Bar,
    Baz
}

string d = FooBarBaz.Foo.GetDescription(); // Some Foo enum
```

## GetDisplayName

Gets value of first ``DisplayAttribute`` for the enum element.

```csharp
private enum FooBarBaz
{
    Foo,
    [Display(Name = "BarBarBar")]
    Bar,
    [Display(Name = "displayNameForEnumBaz", ResourceType = typeof(SomeResources))]
    Baz
}

string d1 = FooBarBaz.Foo.GetDisplayName(); // Foo, because no display name was provided
string d2 = FooBarBaz.Bar.GetDisplayName(); // BarBarBar
string d3 = FooBarBaz.Baz.GetDisplayName(); // Value will be taken from SomeResources file
```
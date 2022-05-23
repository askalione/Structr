# Abstractions

**Structr.Abstractions** package contains number of classes and extension methods to supply wide range of developer basic needs in different situations.

Big part of package consists of extensions for most popular types allowing to avoid redundant code in widespread cases.

For example You need to add elements to dictionary while overriding values for already existing keys, then you choice is [AddRangeOverride](/Extensions/Abstractions-DictionaryExtensions.md) dictionary extension. Or some text should be formatted to hyphen-case style. Then [ToHyphenCase](/Extensions/Abstractions-StringExtensions.md) string extension supplies your needs. Maybe your collection needs some tricky ordering by several fields and directions - advanced [OrderBy](/Extensions/Abstractions-EnumerableExtensions.md) supplies everything needed. Checking input variables is most common case and many of us are bored by typing another if-null-then-throw statements. [Ensure.NotNull](/Abstractions-Ensure.md) (InRange, GreaterThan, etc.) does all this job in one line.

This isn't all. There are extensions for more than ten types, tools for working with async methods, enums, sequential guids, tree-like structures, money types and more. List of all possibilities located below as do some samples.

## Installation

Abstractions package is available on [NuGet](https://www.nuget.org/packages/Structr.Abstractions/). 

```
dotnet add package Structr.Abstractions
```

## Contents

* [Check](/Abstractions-Check.md)
* [Ensure](/Abstractions-Ensure.md)
* [Money](/Abstractions-Money.md)
* [SequentialGuid](/Abstractions-SequentialGuid.md)
* [HierarchyId](/Abstractions-HierarchyId.md)
* Extensions
    * [DateTime](/Extensions/Abstractions-DateTimeExtensions.md)
    * [Dictionary](/Extensions/Abstractions-DictionaryExtensions.md)
    * [DirectoryInfo](/Extensions/Abstractions-DirectoryInfoExtensions.md)
    * [Enumerable](/Extensions/Abstractions-EnumerableExtensions.md)
    * [Enum](/Extensions/Abstractions-EnumExtensions.md)
    * [Expression](/Extensions/Abstractions-ExpressionExtensions.md)
    * [Long](/Extensions/Abstractions-LongExtensions.md)
    * [MemberInfo](/Extensions/Abstractions-MemberInfoExtensions.md)
    * [Object](/Extensions/Abstractions-ObjectExtensions.md)
    * [Queryable](/Extensions/Abstractions-QueryableExtensions.md)
    * [ServiceCollection](/Extensions/Abstractions-ServiceCollectionExtensions.md)
    * [String](/Extensions/Abstractions-StringExtensions.md)
    * [Type](/Extensions/Abstractions-TypeExtensions.md)
* Helpers
    * [AsyncHelper](/Helpers/Abstractions-AsyncHelper.md)
    * [BindHelper](/Helpers/Abstractions-BindHelper.md)

## Samples for some of methods

### AddRangeOverride

Example of mentioned above ``AddRangeOverride`` dictionary extension:

```csharp
var dictionary = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" },
    { 3, "Three" },
    { 4, "Four" }
};
var newDictionary = new Dictionary<int, string>
{
    { 1, "One_overridden" },
    { 3, "Three_overridden" },
    { 5, "Five_new" }
};
dictionary.AddRangeOverride(newDictionary);
```

So after applying extension method will look like:

```csharp
{ 1, "One_overridden" },
{ 2, "Two" },
{ 3, "Three_overridden" },
{ 4, "Four" },
{ 5, "Five_new" }
```

### GetDisplayName

Allows to get Display attribute value for enums:

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

### ToFileSizeString

Converts `long` variable to human readable file size in kilobytes, megabytes etc.

```csharp
12L.ToFileSizeString(); // ---> 12.0 bytes
2200L.ToFileSizeString(); // ---> 2.1 KB
3330000L.ToFileSizeString(); // ---> 3.2 MB
```
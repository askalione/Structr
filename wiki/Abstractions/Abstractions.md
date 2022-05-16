# Structr.Abstractions

Structr.Abstractions package contains number of classes and extension methods to supply wide range of developer basic needs in different situations.

Big part of package consists of extensions for most popular types allowing to avoid redundant code in widespread cases.

For example You need to add elements to dictionary while overriding values for already existing keys, then you choice is [AddRangeOverride](/Extensions/Dictionary.md) dictionary extension. Or some text should be formated to hyphen-case style. Then [ToHyphenCase](/Extensions/StringExtensions.md) string extension supplies your needs. Maybe your collection needs some tricky ordering by several fields and directions - advanced [OrderBy](/Extensions/EnumerableExtensions.md) supplies everything needed. Checking input varibles is most common case and many of us are bored by typing another if-null-then-throw statments. [Ensure.NotNull](/Ensure.md) (InRange, GreaterThan, etc.) does all this job in one line.

This isn't all. There are extensions for more than ten types, tools for working with async methods, enums, sequential guids, tree-like structures, money types and more. List of all possibilities located below as do some samples.

## Installation

Abstractions package is available on [NuGet](https://www.nuget.org/packages/Structr.Abstractions/). 

```
dotnet add package Structr.Abstractions
```

## Contents

* [Check](/Check.md)
* [Ensure](/Ensure.md)
* [Money](/Money.md)
* SequentialGuid
* [HierarchyId](/HierarchyId.md)
* Extensions
    * [DictionaryExtensions](/Extensions/DictionaryExtensions.md)
    * [DirectoryInfoExtensions](/Extensions/DirectoryInfoExtensions.md)
    * [EnumerableExtensions](/Extensions/EnumerableExtensions.md)
    * [EnumExtensions](/Extensions/EnumExtensions.md)
    * [ExpressionExtensions](/Extensions/ExpressionExtensions.md)
    * [LongExtensions](/Extensions/LongExtensions.md)
    * [MemberInfoExtensions](/Extensions/MemberInfoExtensions.md)
    * [ObjectExtensions](/Extensions/ObjectExtensions.md)
    * [QueryableExtensions](/Extensions/QueryableExtensions.md)
    * [StringExtensions](/Extensions/StringExtensions.md)
    * [TypeExtensions](/Extensions/TypeExtensions.md)
    * [ServiceCollectionExtensions](/Extensions/ServiceCollectionExtensions.md)
* Helpers
    * [AsyncHelper](/Helpers/AsyncHelper.md)
    * [BindHelper](/Helpers/BindHelper.md)

## Samples for some of methods

### **AddRangeOverride**
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
    { 1, "One_overriden" },
    { 3, "Three_overriden" },
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

### **GetDisplayName**
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

string d1 = FooBarBaz.Foo.GetDisplayName(); // Foo, cos no display name was provided
string d2 = FooBarBaz.Bar.GetDisplayName(); // BarBarBar
string d3 = FooBarBaz.Baz.GetDisplayName(); // Value will be taken from SomeResources file
```

### **ToFileSizeString**
Converts ``long`` varible to human readable file size in kilobytes, megabytes etc.

```csharp
12.ToFileSizeString(); // gives 12.0 bytes
2200.ToFileSizeString(); // gives 2.1 KB
3330000.ToFileSizeString(); // gives 3.2 MB
```
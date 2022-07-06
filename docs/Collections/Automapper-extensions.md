# Automapper extensions

[Automapper](https://github.com/AutoMapper/AutoMapper)-related extensions for working with different types of collections. 

## Installation

Collections package is available on [NuGet](https://www.nuget.org/packages/Structr.Collections.Extensions.Automapper/). 


```
dotnet add package Structr.Collections.Extensions.Automapper
```

## Usage

Provided extensions allow to perform one-string mappings between collections without a redundant code.

```csharp
var list = new List<Foo>
{
     new Foo { Id = 1, Name = "Bar"},
     new Foo { Id = 2, Name = "Baz"}
};
var mappedCollection = mapper.MapList<FooDto>(list);
```
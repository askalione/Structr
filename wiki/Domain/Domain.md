# Domain

**Structr.Domain** package contains number of classes and interfaces which are intended to help to implement [Domain Model](https://www.domainlanguage.com/ddd/).

## Installation

Domain package is available on [NuGet](https://www.nuget.org/packages/Structr.Domain/).

```
dotnet add package Structr.Domain
```

## Usage

Structr.Domain provides functionality to implement main concepts of domain modelling in your application - [Entities](Domain-Entities.md) and [Value Objects](Domain-ValueObjects.md).

To take full advantage of the implemented domain model with Structr.Domain, if you are using the [Entity Framework 6](https://docs.microsoft.com/en-us/ef/ef6/) or [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) ORM, it is recommended to also use [Structr.EntityFramework](https://www.nuget.org/packages/Structr.EntityFramework/) or [Structr.EntityFrameworkCore](https://www.nuget.org/packages/Structr.EntityFrameworkCore/) packages. The joint use of these components with Structr.Domain allows you to significantly simplify and automate auditing entities, and also opens up opportunities for auto-setup entities configurations in the DbContext.
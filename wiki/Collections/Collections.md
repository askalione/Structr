# Collections

**Structr.Collections** package is intended to help organize search results collections into pagination-friendly arrays, which provide all needed data to display page control buttons in UI.
Instance of package main class - `PageList` - contains data about page number, page size, first and last pages of whole selection, etc.

## Installation

Collections package is available on [NuGet](https://www.nuget.org/packages/Structr.Collections/). 

```
dotnet add package Structr.Collections
```

## Usage

Let's imagine we have some set of goods gotten from database using search criteria "fruits":

```
{ "apple", "banana", "pear", "orange", "mandarin", "plum", "lime", "mango", "cherimoya", "feijoa", "guava", "lemon", "kumquat" };
```

But for some reason we could show only 3 items from search results per page. So, user will get totally 5 pages and currently will see one of them. Let it be the second one. Than after using all skips and takes in our SQL-query we'll get something like this:

```csharp
var fruits = new List<string> { "orange", "mandarin", "plum" };
```

Before sending this result to presentation level, it will be nice to send some pagination info along with `fruits`-list to provide data you need for user control generation purpose. These are: buttons with page numbers, arrows, total count info.
At this moment `PagedList` comes to our help:

```csharp
var result = new PagedList<int>(
     collection: fruits, 
     totalItems: 13, 
     pageNumber: 2, 
     pageSize: 3
);
```

`totalItems` parameter (which is **13**) usually comes from first SQL `COUNT(*)`-like query intended to get total count of elements in search before applying `SKIP` and `TAKE` operators. The second and third parameters here are: `pageNumber` and `pageSize` respectively.
Based on such `result` you can successfully create user interface or provide info about search results to you API-client.

## Properties

| Property name | Property type | Description |
| --- | --- | --- |
| TotalItems | `int` | Gets declared total count of items in superset collection. 
| PageNumber | `int` | Gets current page number. |
| PageSize | `int` | Gets page size. |
| TotalPages | `int` | Gets total count of pages. |
| HasPreviousPage | `bool` | Determines if there is a page before current. |
| HasNextPage | `bool` | Determines if there is a page after current. |
| IsFirstPage | `bool` | Determines whether current page is the first one. |
| IsLastPage | `bool` | Determines whether current page is the last one. |
| FirstItemOnPage | `int` | Gets number of first item on page. |
| LastItemOnPage | `int` | Gets number of last item on page. |
| Count | `int` | Gets count of items on page. |

## Static methods

| Method name | Return type | Description |
| --- | --- | --- |
| Empty | `IPagedList<T>` | Creates an empty paged list. |

## Extensions

To easily get a paged list from the original list use extension methods:

```csharp
var list = new List<string> { "orange", "mandarin", "plum" };
// Option 1: Get `PagedList` with total items count equal to size of original collection
var pagedList = list.ToPagedList(pageNumber: 2, pageSize: 3);
// Option 2: Get `PagedList` with total items comes from `COUNT(*)`-like SQL-query
var pagedList = list.ToPagedList(totalItems: 13, pageNumber: 2, pageSize: 3);
```

It is very common to convert an existing paged list of items of one type (Entities for example) into an paged list of items of another type (DTO for example).

```csharp
// Paged list of entities
var entities = new PagedList<Fruit>(
     new List<Fruit> {
          new Fruit("orange"),
          new Fruit("mandarin"),
          new Fruit("plum")
     }, 
     totalItems: 13, 
     pageNumber: 2,
     pageSize: 3);
// Paged list of DTO
var dto = entities.ToPagedList(_mapper.Map<FruitDto>(entities));
```

For such conversion it is best to use [AutoMapper extensions](Collections-Automapper-extensions.md).
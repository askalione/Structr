## PageBy

Applies pagination to IQueryable instance skipping and taking specified number of elements. Could be efectively used with ORM while constructing query via Linq.

```csharp
var queryable = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();
var result = queryable.PageBy(2, 5); // { 2, 3, 4, 5, 6 }
```

## OrderBy

Sorts the elements of a sequence in order and by properties provided via dictionary. Could help in some difficult cases, when you need, for example, sort by name in ascending order and by age in descending. Number of fields is, of course, could be more than two.

```csharp
var list = new List<FooBar>
{
    new FooBar { Foo = 5, Bar = 1, Baz = 7 },
    new FooBar { Foo = 2, Bar = 3, Baz = 2 },
    new FooBar { Foo = 2, Bar = 2, Baz = 3 },
    new FooBar { Foo = 6, Bar = 3, Baz = 2 },
    new FooBar { Foo = 6, Bar = 3, Baz = 1 },
    new FooBar { Foo = 2, Bar = 4, Baz = 4 }
}.AsQueryable();
var result = list.OrderBy(new Dictionary<string, Order>
{
    { "Foo", Order.Asc },
    { "Bar", Order.Desc },
    { "Baz", Order.Asc }
});

// results in:
// {
//     new FooBar { Foo = 2, Bar = 4, Baz = 4 },
//     new FooBar { Foo = 2, Bar = 3, Baz = 2 },
//     new FooBar { Foo = 2, Bar = 2, Baz = 3 },
//     new FooBar { Foo = 5, Bar = 1, Baz = 7 },
//     new FooBar { Foo = 6, Bar = 3, Baz = 1 },
//     new FooBar { Foo = 6, Bar = 3, Baz = 2 }
// };
```
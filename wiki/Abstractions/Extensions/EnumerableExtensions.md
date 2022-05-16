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
};
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

There are count of more special sorting methods which are not listed here but could be found via IntelliSense.

## PickRandom

Gets a random element from provided collection.

```csharp
var list = new int[] {1, 2, 3};
var result = list.PickRandom(); // 1 or 2 or 3
```

or randomly gets ``count`` items from ``source``:
```csharp

var result = list.PickRandom(2); // 1,2 or 1,3 or 2,3
```
There are count of more special sorting methods which are not listed here but could be found via IntelliSense.

## Shuffle

Shuffles source collection changing its elements positions at random.

```csharp
var list = new int[] {1, 2, 3};
var result = list.Shuffle();
```

## ForEach

Performs simple foreach-like iteration on source collection while invoking provided method for each element of the collection.

```csharp
var list = new List<FooBar>
{
    new FooBar { Foo = 1 },
    new FooBar { Foo = 2 },
    new FooBar { Foo = 3 },
    new FooBar { Foo = 4 }
};
list.ForEach(x => x.Foo = x.Foo + 5);

// results in:
// new FooBar { Foo = 6 },
// new FooBar { Foo = 7 },
// new FooBar { Foo = 8 },
// new FooBar { Foo = 9 }
```

## ForEachOrBreak

Performs simple foreach-like iteration on source collection while invoking provided function for every element of the collection. Breaks iteration when first ``true`` result got from function.

```csharp
var list = new List<FooBar>
{
    new FooBar { Foo = 1 },
    new FooBar { Foo = 2 },
    new FooBar { Foo = 3 },
    new FooBar { Foo = 4 }
};
list.ForEachOrBreak(x =>
{
    if (x.Foo >= 3)
    {
        return true;
    }
    x.Foo = x.Foo + 5;
    return false;
});

// results in:
// new FooBar { Foo = 6 },
// new FooBar { Foo = 7 }, <--- last processed item
// new FooBar { Foo = 3 },
// new FooBar { Foo = 4 }
```
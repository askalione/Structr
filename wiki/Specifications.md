# Specifications

**Structr.Specifications** package provides .NET implementation of [Specification pattern](https://en.wikipedia.org/wiki/Specification_pattern) with set of simple operations with them.

## Installation

Specifications package is available on [NuGet](https://www.nuget.org/packages/Structr.Specifications/).

```
dotnet add package Structr.Specifications
```

## Usage

Main class of package - `Specification<T>` - should be inherited by your own classes in order to use its functionality and extensions.

Example of basic usage:

```csharp
// Some model.
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
}

// Specification matching only books written after some year.
public class BookYearIsGreaterThanSpec : Specification<Book>
{
    public int Year { get; }

    public BookYearIsGreaterThanSpec(int year) 
        => Year = year;

    // The only method to be overridden - ToExpression().
    // It should return expression that gives needed condition.
    public override Expression<Func<Book, bool>> ToExpression()
    {
        return x => x.Year > Year;
    }
}

// Specification matching only books which have price less than some value.
public class BookPriceIsLessThanSpec : Specification<Book>
{
    public int Price { get; }

    public BookPriceIsLessThanSpec(int price)
        => Price = price;

    public override Expression<Func<Book, bool>> ToExpression()
    {
        return x => x.Price < Price;
    }
}
```

Use specifications to filter models collection:

```csharp
// Books collection.
var books = new List<Book>
{
    new Book { Title = "Don Quixote", Author = "Miguel de Cervantes", Year = 1605, Price = 16 },
    new Book { Title = "The Brothers Karamazov", Author = "Fyodor Dostoevsky", Year = 1879, Price = 13 },
    new Book { Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", Year = 1954, Price = 32 },
    new Book { Title = "One Hundred Years of Solitude", Author = "Gabriel Garcia Marquez", Year = 1967, Price = 21 },
};

// Create specification instances.
var newBookSpec = new BookYearIsGreaterThanSpec(1950);
var cheapBookSpec = new BookPriceIsLessThanSpec(25);

// Then filter books collection.
IEnumerable<Book> newBooks = books
    .Where(x => newBookSpec.IsSatisfiedBy(x)); // Gives "The Lord of the Rings" and "One Hundred Years of Solitude".
IEnumerable<Book> cheapBooks = books
    .Where(x => cheapBookSpec.IsSatisfiedBy(x)); // Gives "Don Quixote", "The Brothers Karamazov" and "One Hundred Years of Solitude".

// You also can combine two specifications.
// For example: Cheap and "new" books.
Specification<Book> cheapAndNewBooksSpec = cheapBookSpec.And(newBookSpec);
IEnumerable<Book> cheapAndNewBooks = books
    .Where(x => cheapAndNewBooksSpec.IsSatisfiedBy(x)); // Gives only "One Hundred Years of Solitude".
// For example: Cheap and "old" books.
Specification<Book> cheapAndOldBooksSpec = cheapBookSpec.AndNot(newBookSpec);
IEnumerable<Book> cheapAndOldBooks = books
    .Where(x => cheapAndOldBooksSpec.IsSatisfiedBy(x)); // Gives only "Don Quixote" and "The Brothers Karamazov".
```

The whole list of `Specification<T>` extensions is provided below:

| Method name | Description |
| --- | --- |
| `And<T>` | Creates specification which will be satisfied only when both specifications will be satisfied by provided instance of type `T`. |
| `Or<T>` | Creates specification which will be satisfied when at least one two specifications will be satisfied by provided instance of type `T`. |
| `Not<T>` | Creates specification which will be satisfied when given specification won't be satisfied by provided instance of type `T`. |
| `AndNot<T>` | Creates specification which will be satisfied when first specification will AND second will not be satisfied by provided instance of `T`. |
| `OrNot<T>` | Creates specification which will be satisfied when first specification will OR second will not be satisfied by provided instance of `T`. |

Additionally two predefined specifications are available:

| Name | Description |
| --- | --- |
| `AnySpecification<T>` | Specification to which all objects of `T` will match. |
| `NoneSpecification<T>` | Specification to which none of objects of `T` will match. |

### Entity Framework

Structr.Specifications may be helpful with [Entity Framework 6](https://docs.microsoft.com/en-us/ef/ef6/) or [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/). 
Use `ToExpression()` method with `Where()` for filtering entities in `DbContext`:

```csharp
// Create specification instances.
var newBookSpec = new BookYearIsGreaterThanSpec(1950);
var cheapBookSpec = new BookPriceIsLessThanSpec(25);

// Then filter books entities.
List<Book> newBooks = dbContext.Books
    .Where(x => newBookSpec.ToExpression(x))
    .ToList(); // Gives "The Lord of the Rings" and "One Hundred Years of Solitude".
List<Book> cheapBooks = books
    .Where(x => cheapBookSpec.ToExpression(x))
    .ToList(); // Gives "Don Quixote", "The Brothers Karamazov" and "One Hundred Years of Solitude".
```
# Specification

**Structr.Specification** package provides .NET implementation of Specification pattern with set of simple operations with them.

## Installation

Specification package is available on [NuGet](https://www.nuget.org/packages/Structr.Specification/).

```
dotnet add package Structr.Specification
```

## Setup

No additional setup is needed. Just install the package and use `Specification<T>` base class with its descendants.

## Usage

Main class of package - `Specification<T>` - should be inherited by your own classes in order to use its functionality and extensions.
Let's say we have class describing books - `Book`:

```csharp
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
}
```

Then specification matching only books written after some year will look like:

```csharp
public class NewBookSpec : Specification<Book>
{
    public int Year { get; }
    public FooNameContainsTextSpec(int year) => Year = year;

    // The only method to be overriden - ToExpression().
    // It should return expression that gives needed condition.
    public override Expression<Func<Book, bool>> ToExpression()
    {
        return x => x.Year >= year;
    }
}
```

And for cheap books it will be:

```csharp
public class CheapBookSpec : Specification<Book>
{
    public override Expression<Func<Book, bool>> ToExpression()
    {
        return x => x.Price <= 25;
    }
}
```

Now we could use both specifications to filter our books list:

```csharp
var books = new List<Book>
{
  new Book {Title = "Don Quixote", Author = "Miguel de Cervantes", Year = 1605, Price = 16},
  new Book {Title = "The Brothers Karamazov", Author = "Fyodor Dostoevsky", Year = 1879, Price = 13},
  new Book {Title = "The Lord of the Rings", Author = "J.R.R. Tolkien", Year = 1954, Price = 32},
  new Book {Title = "One Hundred Years of Solitude", Author = "Gabriel Garcia Marquez", Year = 1967, Price = 21},
};

// Let's take only cheap books.
var cheapBooks = books
  .Where(x => new CheapBookSpec().IsSatisfiedBy(x)).ToList(); // Gives "Don Quixote", "The Brothers Karamazov" and "One Hundred Years of Solitude".

// Then only books written after year 1950.
var newBooks = books
  .Where(x => new NewBookSpec(1950).IsSatisfiedBy(x)).ToList(); // Gives "The Lord of the Rings" and "One Hundred Years of Solitude".

// Now we could combine this two specifications in different ways:
// Cheap and "new" books:
var cheapAndNewBooksSpec = new CheapBookSpec().And(new NewBookSpec(1950));
var cheapAndNewBooks = books
  .Where(x => cheapAndNewBooksSpec.IsSatisfiedBy(x)).ToList(); // Gives only "One Hundred Years of Solitude".

// Cheap and old books:
var cheapAndOldBooksSpec = new CheapBookSpec().AndNot(new NewBookSpec(1950));
var cheapAndOldBooks = books
  .Where(x => cheapAndOldBooksSpec.IsSatisfiedBy(x)).ToList(); // Gives only "Don Quixote" and "The Brothers Karamazov".
```

The whole list of extensions is provided below:

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
# Money

`Money<T>` is a base class to be used for operations which include counting money. Provides necessary tools for working with money-like variables.

Its using implies creating your own money class that specifies base simple type to store money value in. The usage examples are provided below.

## Basic usage

First of all, some application-specific money class should be defined. Overriding of basic math operators is needed here because basic class didn't know about `long` type and thus about basic operations with it.

```csharp
public class Money : Money<long>
{
    public Money(long val) : base(val) { }        
    public static Money operator +(Money money1, Money money2)
    {
        return new Money(money1.Value + money2.Value);
    }
    public static Money operator -(Money money1, Money money2)
    {
        return new Money(money1.Value - money2.Value);
    }
}
```

All other stuff such as comparison and equality is already defined in base class.

```csharp
Money money = new MyMoney(10);
Money sameMoney = new MyMoney(10);
var isEqual = money == sameMoney; // ---> true

Money moreMoney = new MyMoney(15);
var isLess = money < moreMoney; // ---> true

// etc.
```

Money class allows to use currencies is also available:

```csharp
public enum Currency
{
    CNY,
    EUR,
    GBP,
    RUB,
    USD
}
class Money : Money<long, Currency>
{
    public Money(long val, Currency currency) : base(val, currency) { }
}
```

All operations will be performed considering currency.
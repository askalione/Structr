## Value objects

Value object - is an unchangeable object that has attributes, but no distinct identity. In DDD it is very important to distinguish between Entities and Value Objects. The main difference between `Entity` and `ValueObject` is in the method `Equals`. Two entities are considered equal if their identifiers are equal. The `ValueObject` has no identifier. Two `ValueObjects` are considered equal if all their public properties are equal in value.

Example of value object:

```csharp
public class Address : ValueObject<Address>
{
    public string City { get; set; }
    public string Street { get; set; }
    public string House { get; set; }
}
```

Use `Address` value object in `User` entity:

```csharp
public class User : Entity<User, int>
{
    public string Name { get; private set; }
    public Address Address { get; private set; }

    private User() : base() { }

    public User(string name, Address address) : this()
    {
        Name = name;
        Address = address;
    }

    public void ChangeAddress(Address address)
    {
        Address = address;
    }
}
```

Example of value objects comparing:

```csharp
bool isEqual;

var address1 = new Address { City = "Moscow", Street = "Prospekt Mira", House = "57" };
var address2 = new Address { City = "Moscow", Street = "Prospekt Mira", House = "57" };
var address3 = new Address { City = "Volgograd", Street = "Prospekt Mira", House = "57" };

isEqual = address1 == address2; // Returns "true"
isEqual = address1 == address3; // Returns "false" because of different cities
```
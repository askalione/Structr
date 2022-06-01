# Domain

**Structr.Domain** package is intended to organize Domain Model (DDD) in application.

## Installation

Domain package is available on [NuGet](https://www.nuget.org/packages/Structr.Domain/).

```
dotnet add package Structr.Domain
```

## Class `Entity`

Class `Entity<TEntity, TKey>` is a general class for implementing a domain entity `TEntity` with identifier `TKey`.

For example, you can create a class `User` inherited from the class `Entity<User, int>` with identifier `int`:

```csharp
public class User : Entity<User, int>
{
    public string Fio { get; private set; }

    private User() : base() { }

    public User(int id, string fio) : this()
    {
        if (string.IsNullOrWhiteSpace(fio))
        {
            throw new ArgumentNullException(nameof(fio));
        }

        Id = id;
        Fio = fio.Trim();
    }
}
```

You can use enum for identifier like this:

```csharp
public enum RoleId
{
    User,
    Admin,
}

public class Role : Entity<Role, RoleId>
{
    public string Name { get; private set; }

    private Role() : base() { }

    public Role(RoleId id, string name) : this()
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Id = id;
        Name = name.Trim();
    }
}
```

## Auditable interfaces

You can use for entity one or more of the following auditable interfaces:

* `IAuditable` - general interface for an auditable entity.
* `ICreatable` - provides information about an auditable entity creation date.
* `ISignedCreatable` - provides information about who created an auditable entity and when.
* `IModifiable` - provides information about an auditable entity modification date.
* `ISignedModifiable` - provides information about who modified an auditable entity and when.
* `IUndeletable` - general interface for an undeletable auditable entity.
* `ISoftDeletable` - provides information about an auditable entity deletion date.
* `ISignedSoftDeletable` - provides information about who deleted an auditable entity and when.

For example, you can add the interface `ICreatable` to the class `User` and add the field `DateCreated` to define the creation date of the entity:

```csharp
public class User : Entity<User, int>, ICreatable
{
    public string Fio { get; private set; }
    public DateTime DateCreated { get; private set; }

    private User() : base() { }

    public User(int id, string fio) : this()
    {
        if (string.IsNullOrWhiteSpace(fio))
        {
            throw new ArgumentNullException(nameof(fio));
        }

        Id = id;
        Fio = fio.Trim();
    }
}
```

## `AuditableEntity` and `SignedAuditableEntity` classes

Class `AuditableEntity<TEntity, TKey>` provides `ICreatable` and `IModifiable` interfaces.

Class `SignedAuditableEntity<TEntity, TKey>` provides `ISignedCreatable` and `ISignedModifiable` interfaces.

## Class `CompositeEntity`

Class `CompositeEntity<TEntity>` is a general class for an entity <see cref="TEntity"/> with composite identifier.

For example, you can create a class `UserRole` inherited from the class `CompositeEntity<UserRole>` and override method `GetCompositeId()` with composite identifier `(int, int)`:

```csharp
public class UserRole : CompositeEntity<UserRole>
{
    public int UserId { get; private set; }
    public virtual User User { get; private set; }

    public RoleId RoleId { get; private set; }
    public virtual Role Role { get; private set; }

    private UserRole() : base() { }

    internal UserRole(User user, Role role) : this()
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        UserId = user.Id;
        User = user;

        RoleId = role.Id;
        Role = role;
    }

    public static Expression<Func<UserRole, object>> CompositeId
        => x => new { x.UserId, x.RoleId };

    protected override object GetCompositeId()
        => CompositeId.Compile().Invoke(this);
}
```

## Class `ValueObject`

The main difference between `Entity` and `ValueObject` is in the method `Equals`. Two entities are considered equal if and only if their identifiers are equal. The `ValueObject` has no identifier. Two `ValueObjects` are considered equal if and only if all their public properties are equal in value.

For example, you can create class `Address`:

```csharp
    public class Address : ValueObject<Address>
    {
        public string Town { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
    }
```

And then use `Address` in `User`:

```csharp
public class User : Entity<User, int>
{
    public string Fio { get; private set; }
    public Address Address { get; private set; }

    private User() : base() { }

    public User(int id, string fio, Address address) : this()
    {
        if (string.IsNullOrWhiteSpace(fio))
        {
            throw new ArgumentNullException(nameof(fio));
        }
        if (address == null)
        {
            throw new ArgumentNullException(nameof(address));
        }

        Id = id;
        Fio = fio.Trim();
        Address = address;
    }
}
```
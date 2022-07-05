# Entities

Entities are domain objects that are uniquely defined by a unique identifier, and not by their attributes.

To implement new entity in your application inherit entity class from [Entity<TEntity, TKey>](#entity) or [CompositeEntity<TEntity>](#compositeentity).

## Entity

`Entity<TEntity, TKey>` is base class for an entity `TEntity` with unique identifier `TKey`.

Properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Id | `TKey` | The entity unique identifier. |

Methods:

| Method name | Return type | Description |
| --- | --- | --- |
| IsTransient | `bool` | Returns `true` if current entity is transient (created in memory but not saved in data store), otherwise `false`. |
| Equals | `bool` | Indicates whether the current entity is equal to specified entity of the same type. |

`Entity<TEntity, TKey>` also have implementation of operators `==` and `!=`.

Example of simple entity:

```csharp
// User entity with unique identifier property "int Id { get; protected set; }".
public class User : Entity<User, int>
{
    public string Name { get; private set; }

    private User() : base() { }

    public User(int id, string name) : base()
    {
        Id = id;
        Name = name;
    }
}
```

Let's compare two object of type `User`:

```csharp
bool isEqual;

// Entities with different identifiers:
var user1 = new User(1, "Robert");
var user2 = new User(2, "Robert");

isEqual = user1 == user2; // Returns "false" because "user1.Id" equals "1", but "user2.Id" equals "2".

// Entities with same identifiers:
var user3 = new User(3, "Emma");
var user4 = new User(3, "Leo");

isEqual = user3 == user4; // Returns "true" because their identifiers are equals.
```

You can also use `enum`, `Guid`, `string` and other types for identifier:

```csharp
public enum RoleId
{
    Guest,
    Admin
}

public class Role : Entity<Role, RoleId>
{
    public string Name { get; private set; }

    private Role() : base() { }

    public Role(RoleId id, string name) : this()
    {
        Id = id;
        Name = name;
    }
}
```

In real world when you using ORM (like EF6 or EFCore) on board you don't setting entity identifier in constructor for auto-increment primary keys. It's because ORM do it when you call `SaveChanges()`. That's why you may need method `IsTransient()`.

Example:

```csharp
var user = new User("Olaf");

int currentUserId = user.Id; // Returns "0", because you don't set identifier in constructor;
bool isTransient = user.IsTransient(); // Returns "true"

// Use DbContext to save user in database
using(var dbContext = new UserDbContext())
{
    dbContext.Users.Add(user);
    dbContext.SaveChanges();
}

int currentUserId = user.Id; // Returns "1", because auto-incremented;
bool isTransient = user.IsTransient(); // Returns "false"
```

## CompositeEntity

`CompositeEntity<TEntity>` is base class for an entity `TEntity` with composite unique identifier.

`CompositeEntity<TEntity>` have all the same methods as `Entity<TEntity, TKey>`, but don't have and `Id` property. Instead of `public` property `Id` that class have `protected abstract` method `GetCompositeId()` that should be implementing to resolve composite identifier.

Using this class is very helpful when setting up entity type configurations in EF6 or EFCore DbContext.

Example:

```csharp
public class UserRole : CompositeEntity<UserRole>
{
    public int UserId { get; private set; }
    public virtual User User { get; private set; }

    public RoleId RoleId { get; private set; }
    public virtual Role Role { get; private set; }

    private UserRole() : base() { }

    public UserRole(User user, Role role) : this()
    {
        UserId = user.Id;
        User = user;

        RoleId = role.Id;
        Role = role;
    }

    // Trick for setting up entity type configuration in ORM DbContext.
    public static Expression<Func<UserRole, object>> CompositeId
        => x => new { x.UserId, x.RoleId };

    protected override object GetCompositeId()
        => CompositeId.Compile().Invoke(this);
}
```

And then simply setting up entity type configuration in DbContext, for example EFCore:

```csharp
internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Use tricky property.
        builder.HasKey(UserRole.CompositeId);

        /* Other settings */
    }
}
```

## Auto-auditing

Structr.Domain provides some abstract classes and interfaces to easy implement auto-auditing in ORM DbContext.

Auto-auditing - it's when you call `SaveChanges()` entity's properties like `DateCreated` or `DateModified` filling and tracking automatically.

List of auditable interfaces and abstract classes:

| Type name | Description |
| --- | --- |
| `ICreatable` | Provides information about an auditable entity creation date. |
| `IModifiable` | Provides information about an auditable entity modification date. |
| `ISoftDeletable` | Provides information about an auditable entity deletion date. |
| `ISignedCreatable` | Provides information about who created an auditable entity and when. |
| `ISignedModifiable` | Provides information about who modified an auditable entity and when. |
| `ISignedSoftDeletable` | Provides information about who deleted an auditable entity and when. |
| `AuditableEntity` | Provides information about an auditable entity creation date and modification date. |
| `SignedAuditableEntity` | Provides information about who created and modified an auditable entity and when. |

Example of usage:

```csharp
public class User : Entity<User, int>, ICreatable
{
    public string Name { get; private set; }
    // Auditable property.
    public DateTime DateCreated { get; private set; }

    private User() : base() { }

    public User(string name) : this()
    {
        Name = name;
    }
}
```

Configure your `DbContext` to use auto-auditing:

```csharp
public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Apply entity type configurations for auditable interfaces.
        builder.ApplyAuditableConfiguration();
    } 

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        Audit();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        Audit();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void Audit()
        => this.Audit(() => DateTime.Now); // Use "Audit()" extension method with setting time stamp provider.
}
```

**Important**: For using Structr.Domain benefits with DbContext you should install [Structr.EntityFramework](#) or [Structr.EntityFrameworkCore](#) package.
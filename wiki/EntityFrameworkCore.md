# EntityFrameworkCore

**Structr.EntityFrameworkCore** package provides methods to easy implement auto-auditing in EntityFrameworkCore `DbContext`.

## Installation

Structr.EntityFrameworkCore package is available on [NuGet](https://www.nuget.org/packages/Structr.EntityFrameworkCore/).

```
dotnet add package Structr.EntityFrameworkCore
```

## Usage

Structr.EntityFrameworkCore package uses with [Structr.Domain](Domain/Domain.md) package that provides some abstract classes for entities and value objects and interfaces like `ICreatable` or `ISignedCreatable`. Structr.EntityFrameworkCore provides methods to easy implement auto-auditing in EFCore DbContext. Auto-auditing - it's when you call `SaveChanges()` entity's properties like `DateCreated` or `CreatedBy` filling and tracking automatically.

For example, create some entity that inherits `Entity<TEntity,TKey>` class and `ISignedCreatable` interface.

```csharp
public class Foo : Entity<Foo, int>, ISignedCreatable
{
    public string Name { get; set; }
    public string CreatedBy { get; set; }
    public DateTime DateCreated { get; set; }
}
```

Create your own `DbContext` class, configure it with `ITimestampProvider` and `IPrincipal` and add `DbSet<Foo>`.

```csharp
public class DataContext : DbContext
{
    public DbSet<Foo> Foos { get; private set; } = default!;

    private readonly ITimestampProvider _timestampProvider;
    private readonly IPrincipal _principal;

    private AuditTimestampProvider _auditTimestampProvider => _timestampProvider != null
        ? _timestampProvider.GetTimestamp
        : null;
    private AuditSignProvider _auditSignProvider => _principal != null
        ? () => _principal.Identity.Name
        : null;

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        _timestampProvider = options.GetService<ITimestampProvider>();
        _principal = options.GetService<IPrincipal>();
    }
    ...
}
```

Then override `OnModelCreating()` method and use `ApplyEntityConfiguration()`, `ApplyValueObjectConfiguration()` and/or `ApplyAuditableConfiguration()`.

```csharp
public class DataContext : DbContext
{
...
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyEntityConfiguration();
        builder.ApplyValueObjectConfiguration();
        builder.ApplyAuditableConfiguration();
    }
...
}
```

Finally override `SaveChanges()` and `SaveChangesAsync()` methods and use `Audit()` extension method for `DbContext`.

```csharp
public class DataContext : DbContext
{
...
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.Audit(_auditTimestampProvider, _auditSignProvider);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.Audit(_auditTimestampProvider, _auditSignProvider);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
```

Now if you add a new instance of `Foo` to `DbSet<Foo>` and call `SaveChanges()` or `SaveChangesAsync()`, the `DateCreated` and `CreatedBy` instance properties will be filled in automatically.

## Options

You can use `EntityConfigurationOptions` or `ValueObjectConfigurationOptions` to configure all descendants of `Entity<TEntity, TKey>` or `ValueObject<TValueObject>`, for example, you can add prefix for all names of value object properties.

```csharp
builder.ApplyValueObjectConfiguration(options =>
{
    options.Configure = (entityType, navigationName, builder) =>
    {
        foreach (var property in entityType.GetProperties())
        {
            property.SetColumnName("prefix_" + property.Name);
        }
    };
});
```

You can use `AuditableConfigurationOptions` to configure signed properties for auditable entities, for example:

```csharp
builder.ApplyAuditableConfiguration(options =>
{
    options.SignedColumnIsRequired = true;
    options.SignedColumnMaxLength = 100;
});
```
# EntityFramework

**Structr.EntityFramework** package provides methods to easy implement auto-auditing in [Entity Framework](https://docs.microsoft.com/en-us/ef/ef6/) `DbContext`, `ModelBuilder` extensions which applies the entity type default configuration and `IQueryable<T>` extensions to paginate through a list of entities.

## Installation

EntityFramework package is available on [NuGet](https://www.nuget.org/packages/Structr.EntityFramework/).

```
dotnet add package Structr.EntityFramework
```

## Usage

EntityFramework package have a reference to [Structr.Domain](Domain/Domain.md) package that provides some abstract classes for entities, value objects and interfaces which are intended to help to implement [Domain Model](https://www.domainlanguage.com/ddd/).

### Auto-auditing

Auto-auditing - it's entity change auditing for Entity Framework entities. The basic process for auditing is to override the `SaveChanges()` method on the `DbContext` and plug in some logic to filling and tracking auditable properties (e.g. `DateCreated` or `CreatedBy`) automatically.

For example, create some entity that inherits `Entity<TEntity,TKey>` class and `ICreatable` interface.

```csharp
public class Issue : Entity<Issue, int>, ICreatable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; } // It's an auditable property from "ICreatable".
}
```

Create your own `DbContext` class implementation with configure some auditing dependencies (e.g. `ITimestampProvider`) and override `SaveChanges()` methods.

```csharp
public class DataContext : DbContext
{
    public DbSet<Issue> Issues { get; set; };

    private readonly AuditTimestampProvider? _auditTimestampProvider;

    public DataContext(string nameOrConnectionString, ITimestampProvider? timestampProvider = null) 
        : base(nameOrConnectionString)
    {
        _auditTimestampProvider = timestampProvider != null ? timestampProvider.GetTimestamp : null;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.Audit(_auditTimestampProvider); // Auto-audit entities.
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        this.Audit(_auditTimestampProvider); // Auto-audit entities.
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
```

Now if you add a new instance of `Issue` to `DbSet<Issue>` and call `SaveChanges()` or `SaveChangesAsync()` method, the `DateCreated` property will be filled in automatically.

### Auto-configure entity types

Create your own `DbContext` class implementation and override `OnModelCreating()` method with calling `ApplyEntityConfiguration()`, `ApplyValueObjectConfiguration()` and/or `ApplyAuditableConfiguration()` methods.

```csharp
public class DataContext : DbContext
{
    public DataContext(string nameOrConnectionString) 
    : base(nameOrConnectionString)
    {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyEntityConfiguration();
        builder.ApplyValueObjectConfiguration();
        builder.ApplyAuditableConfiguration();
    }
}
```

`ModelBuilder` extension methods list:

| Method name | Description |
| --- | --- |
| ApplyEntityConfiguration | Applies the default configuration for all classes inherited from the `Entity<TEntity, TKey>`. Automatically configures PK for entities with `HasKey()` method. | 
| ApplyValueObjectConfiguration | Applies the default configuration for all classes inherited from the `ValueObject<TValueObject>`. |
| ApplyAuditableConfiguration | Applies the default configuration for all classes that implement the `IAuditable`. |

You can configure options when applies entity types default configurations.
For example, configure PK column name for all domain entities:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.ApplyEntityConfiguration(options =>
    {
        options.Configure = (typeConfiguration) =>
        {
            typeConfiguration.Property("Id")
                .HasColumnName($"{typeConfiguration.ClrType.Name}ID");
        };
    });
}
```

`EntityConfigurationOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Configure | `Action<IMutableEntityType, EntityTypeBuilder>` | Delegate for configuring Entities.. Default value is `null`. |

Configure name strategy for all properties of value objects:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.ApplyValueObjectConfiguration(options =>
    {
        options.Configure = (typeConfiguration) =>
        {
            foreach (var property in typeConfiguration.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                typeConfiguration.Property(property).HasColumnName(property.Name);
            }
        };
    });
}
```

`ValueObjectConfigurationOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Configure | `Action<IMutableEntityType, string, OwnedNavigationBuilder>` | Delegate for configuring Value Objects. Default value is `null`. |

Configure signed properties for auditable entities:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.ApplyAuditableConfiguration(options =>
    {
        options.SignedColumnIsRequired = true;
        options.SignedColumnMaxLength = 100;
    });
}
```

`AuditableConfigurationOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| SignedColumnMaxLength | `int` | Defines the maximum size of a signed column (`CreatedBy`, `ModifiedBy`, `DeletedBy`). Default value is `50`. |
| SignedColumnIsRequired | `bool` | Defines if a signed column (`CreatedBy`, `ModifiedBy`, `DeletedBy`) is required. Default value is `false`. |

### Pagination

Use `.ToPagedList()` and `.ToPagedListAsync()` extension methods for `IQueryable<T>` to paginate result of entities query:

```csharp
PagedList<Issue> issues = _dbContext.Issues
    .OrderByDescending(x => x.DateCreated)
    .ToPagedList(pageNumber: 1, pageSize: 10); // Returns only top 10 of latest issues.
```

If you call `ToPagedList()` method with `pageSize` parameter equals `-1` that returns all entities from `DbContext` without limitation. 
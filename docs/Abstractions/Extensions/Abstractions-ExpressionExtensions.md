# Expression extensions

## GetPropertyName

Gets property name by expression. Can be used for nested properties.

```csharp
Expression<Func<Foo, int>> propertyExpression = x => x.BarProperty.BarId;
var result = propertyExpression.GetPropertyName(); // gives "BarProperty.BarId"
```

## GetMember

Gets `MemberInfo` instance for property provided via expression.

```csharp
Expression<Func<Foo, int>> propertyExpression = x => x.BarProperty.BarId;
var result = propertyExpression.GetMember(); // gives MemberInfo instance for BarId
```

## MakeNonGeneric

Makes non generic function from generic.

```csharp
Func<Foo, Bar> func = x => x.BarProperty;
var result = func.MakeNonGeneric(); // gives instance of type Func<object, object>
```
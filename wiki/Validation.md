# Validation

**Structr.Validation** package contains number of attributes supplying a wide range of developer's basic needs in validating user-input data. Everything is done in classic manner by specifying validation attributes for properties to be validated. Every attribute generates it's own but fully customizable error message. More to say - validation attributes could be combined between each other when you need it. 

## Installation

Abstractions package is available on [NuGet](https://www.nuget.org/packages/Structr.Validation/). 

```
dotnet add package Structr.Validation
```

## Basic usage

To use all abilities of this package you just need to register corresponding service in your application:

```csharp
serviceCollection.AddAspNetCoreValidation();
```

Then just add verification attributes anywhere you need them. For example:

```csharp
public class AddUserVm : IViewModel
{
    public string FullName { get; set; }
    public bool IsEmployee { get; set; }

    [RequiredIf(nameof(IsEmployee), true, ErrorMessage = "Employee must have some role.")]
    public IEnumerable<int> RolesIds { get; set; }

    public DateTime? ActiveFrom { get; set; }

    [GreaterThanOrEqualTo(nameof(ActiveFrom), PassOnNull = true)]
    public DateTime? ActiveTo { get; set; }
}
```

All of attributes allow using of properties described below:

| Attribute name | Description |
| --- | --- |
| `RelatedProperty` | Value of the related property with which the value of validating property will be compared.
| `RelatedPropertyDisplayName` | Display name of the related property with which value the value of validating property will be compared. Will be used in error message if validation fails.
| `ErrorMessage` | An error message to associate with a validation control if validation fails.
| `ErrorMessageResourceName` | The error message resource name to use in order to look up the `ErrorMessageResourceType` property value if validation fails.
| `ErrorMessageResourceType` | The resource type to use for error-message lookup if validation fails.

So all attributes allow to specify custom error messages and get them from resource files if needed.

## Conditional validation attributes

These attributes allow to specify related property which value will be used to check value of marked property.

| Attribute name | Description |
| --- | --- |
| `EqualTo` | Specifies that a data field value must be equal to a value of specified related property.
| `NotEqualTo` | Specifies that a data field value must NOT be equal to a value of specified related property.
| `GreaterThan` | Specifies that a data field value must be greater than a value of specified related property.
| `GreaterThanOrEqual` | Specifies that a data field value must be greater or equal to a value of specified related property.
| `LessThan` | Specifies that a data field value must be less than a value of specified related property.
| `LessThanOrEqualTo` | Specifies that a data field value must be less or equal to a value of specified related property.
| `In` | Specifies that a data field value must be contained in value of specified related property. In case of array-type of related property simple inclusion will be checked. If it's not an array-type the equality operator will be used.
| `NotIn` | Specifies that a data field value must NOT be contained in value of specified related property. In case of array-type of related property simple inclusion will be checked. If it's not an array-type the equality operator will be used.
| `Is` | The generalized version of those above. Specifies that a data field value must match a value of specified related property. This one needs a matching `Operator` to be specified.

In addition to properties available for all validation attributes, attributes of this type has `PassOnNull`. This property indicates that validation should be passed if value of property to be validated or value of related property equals `null`. In case of both values are `null` then the behavior of validation will depend on type of attribute. `GreaterThan`, `LessThan`, `NotIn` and `NotEqualTo` will fail validation. Others will succeed.

## Conditional requirement attributes

These allow to make marked property requirement as conditional and based on related property value.

| Attribute name | Description |
| --- | --- |
| `RequiredIfEmpty` | Marks property as required when related property is empty.
| `RequiredIfNotEmpty` | Marks property as required when related property is NOT empty.
| `RequiredIfTrue` | Marks property as required when related property equals `true`.
| `RequiredIfFalse` | Marks property as required when related property equals `false`.
| `RequiredIfRegExMatch` | Marks property as required when related property matches provided regular expression.
| `RequiredIfNotRegExMatch` | Marks property as required when related property DOESN'T match provided regular expression.
| `RequiredIf` | The generalized version of attributes above.

## Conditional checking with regular expression

The last one is `RegularExpressionIf` attribute which allows to check that a data field value must match the specified regular expression but only when related property has specified value.

```csharp
public class SomeModel
{
    [RegularExpressionIf("[A-Z][a-z]\\d", "Bar", true)]
    public string Foo { get; set; }

    public bool Bar { get; set; }
}
```

This one will validate `Foo` property to match `[A-Z][a-z]\d` expression only if `Bar` property will be `true`.
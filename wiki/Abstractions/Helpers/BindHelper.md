# BindHelper

`BindHelper` provides functionality for generating objects on base of some Enum, while binding data contained in enum attributes to coresponding properties of specified objects.

## Usage

One of common usecases of this helper is a permission-objects creating objects based on corresponding enumeration.

Let's say we've got some enumeration containing permission items: 

```csharp
public enum PermissionId
{
    [BindProperty("Name", "User details")]
    [BindProperty("Description", "Permission to view user details")]
    UserDetails,
    [BindProperty("Name", "Create new user")]
    [BindProperty("Description", "Permission to create new user")]
    UserCreate,
    [BindProperty("Name", "Edit user")]
    [BindProperty("Description", "Permission to edit user")]
    UserEdit,
    /* etc. */
}
```
It's relatively simple to manage application users permissions in such way, but what to do if some asks to change name or description of permission?

Here comes permission-describing class which instances should be stored in DB. This will provide you with possibility to change their names and descriptions without touching source code:

```csharp
public class Permission
{
    public int Id { get; private set; }
    public string Value { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; };
}
```
Last question is how to populate such instances during first DB-seeding or when we have updated application and changed some permission?
Here `BindHelper` comes to our aid - with it we can do such job in one statment:

```csharp
var result = BindHelper.Bind<Permission, PermissionId>((obj, enum) =>
{
    obj.Value = enum.ToString();
});
```

This returns the list of `Permission`-objects equivalent to:

```csharp
{
    new Permisson
    { 
        Value = "UserDetails",
        Name = "User details",
        Description = "Permission to view user details"
    },
    new Permisson
    { 
        Value = "UserCreate",
        Name = "Create new user",
        Description = "Permission to create new user"
    },
    /* etc. */
}
```

`Bind` allows to specify method which will be used during creation process and populating properties. All other properties values will be taken from `BindProperty` attributes marking enum values.
`BindPropertyAttribute` also allows to specify nested property to bind with using dots in property name. For example "Name.Eng".
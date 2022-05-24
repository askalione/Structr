# Navigation

**Structr.Navigation** package is intended to help organize navigation menu (nav bar) or/and breadcrumbs in web-application.

## Installation

Navigation package is available on [NuGet](https://www.nuget.org/packages/Structr.Navigation/).

```
dotnet add package Structr.Navigation
```

## Setup

Create menu or breadcrumb navigation item.

```csharp
public class MenuItem : NavigationItem<MenuItem>
{
	public string Action { get; set; }
	public string Controller { get; set; }
	public string Area { get; set; }
	public string Icon { get; set; }
}
```

Navigation services uses different providers to get source navigation data. For example: JSON, XML file, Database, or something else.

You can create custom navigation provider:

```csharp
public class CustomNavigationProvider<TNavigationItem> : INavigationProvider<TNavigationItem>
    where TNavigationItem : NavigationItem<TNavigationItem>, new()
{
    public IEnumerable<TNavigationItem> CreateNavigation() 
    {
        /* Do some logic here */
    }
}
```

And then setup navigation services:

```csharp
services.AddNavigation()
    .AddProvider(new CustomNavigationProvider<MenuItem>());
```

Or you can use one of default implemented navigation provider from list:

* [JSON](#json-provider)
* [XML](#xml-provider)

### JSON provider

Create JSON file with hierarchical navigation:

```json
[
  {
    "Id": "Parent_1",
    "Title": "Parent 1",
    "Icon": "icon-1",
    "Action": "Parent_1_Action",
    "Controller": "Parent_1_Controller",
    "Children": [
      {
        "Id": "Child_1_1",
        "Title": "Child 1 1",
        "Icon": "icon-1-1",
        "Action": "Child_1_1_Action",
        "Controller": "Child_1_1_Controller"
      }
    ]
  },
  {
    "Id": "Parent_2",
    "Title": "Parent 2",
    "Icon": "icon-2",
    "Action": "Parent_2_Action",
    "Controller": "Parent_2_Controller"
  },
  {
    "Id": "Parent_3",
    "Title": "Parent 3",
    "Icon": "icon-3",
    "Action": "Parent_3_Action",
    "Controller": "Parent_3_Controller"
  }
]
```

Setup JSON navigation provider:

```csharp
services.AddNavigation()
    .AddJson<MenuItem>("path_to_json_file");
```

### XML provider

Create XML file with hierarchical navigation: 

```xml
<?xml version="1.0" encoding="utf-8" ?>
<menu>
    <item id="Parent_1" title="Parent 1" Action="Parent_1_Action" Controller="Parent_1_Controller" icon="icon-1">
        <item id="Child_1_1" title="Child 1 1" Action="Child_1_1_Action" Controller="Child_1_1_Controller" icon="icon-1-1"/>
    </item>
    <item id="Parent_2" title="Parent 2" Action="Parent_2_Action" Controller="Parent_2_Controller" icon="icon-2"/>
    <item id="Parent_3" title="Parent 3" Action="Parent_3_Action" Controller="Parent_3_Controller" icon="icon-3"/>
</menu>
```

Setup XML navigation provider:

```csharp
services.AddNavigation()
    .AddXml<MenuItem>("path_to_xml_file");
```

### Options

When you setup navigation provider you can configure navigation options represents by `NavigationOptions<TNavigationItem>`.

`NavigationOptions<TNavigationItem>` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| ResourceType | `Type` | Determines a type of resources file whether uses for localization, `null` by default. | 
| ItemFilter | `Func<TNavigationItem, bool>` | Determines a filter function for navigation items, `item => true` by default. | 
| ItemActivator | `Func<TNavigationItem, bool>` | Determines an activation function for navigation items, `item => false` by default. | 
| EnableCaching | `bool` | Determines whether navigation items should be cached, `true` by default. | 

Example configure navigation services:

```csharp
services.AddNavigation()
    .AddJson<MenuItem>("path_to_json_file", (serviceProvider, options) =>
    {
        options.ResourceType = typeof(MenuResource); // Also navigation item should have configured `ResourceName` property.
        options.ItemFilter =
            item => serviceProvider.GetService<IMenuFilter>().Filter(item);
        options.ItemActivator =
            item => serviceProvider.GetService<IMenuActivator>().Activate(item);
    });
```

## Usage

Navigation services uses to organize [menu](/Navigation-Menu.md) or [breadcrumbs](/Navigation-Breadcrumbs.md).
Both of navigation elements should be inherited from `NavigationItem<T>` that represents basic navigation item.

`NavigationItem<T>` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Id | `string` | Navigation item identifier. | 
| Title | `string` | Navigation item title. | 
| ResourceName | `string` | The key of navigation item in resource file. | 
| Children | `IEnumerable<TNavigationItem>` | Child navigation elements. | Returns `true` if the navigation item has an active descendant, otherwise returns `false`.
| Ancestors | `IEnumerable<TNavigationItem>` | Returns all parent navigation items. | 
| Descendants | `IEnumerable<TNavigationItem>` | Returns all child navigation items. | 
| Parent | `TNavigationItem` | Returns closest parent navigation item. | 
| IsActive | `bool` | Status of navigation item. Only one navigation item can be active at the same time. | 
| HasChildren | `bool` | Returns `true` if the navigation item has a child, otherwise returns `false`. | 
| HasActiveChild | `bool` | Returns `true` if the navigation item has an active child, otherwise returns `false`. | 
| HasActiveDescendant | `bool` | Returns `true` if the navigation item has an active descendant, otherwise returns `false`. | 
| HasActiveAncestor | `bool` | Returns `true` if the navigation item has an active ancestor, otherwise returns `false`. | 

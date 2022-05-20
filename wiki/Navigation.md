# Structr.Navigation

Structr.Navigation package is designed to implement a navigation menu (nav bar) or/and breadcrumbs in your ASP.NET application.

## Installation

Navigation package is available on [NuGet](https://www.nuget.org/packages/Structr.Navigation/).

```
dotnet add package Structr.Navigation
```

## Usage example in ASP.NET MVC project with JSON navigation configuration file

1. Create your navigation item class, for example, `MenuItem`:

```
public class MenuItem : NavigationItem<MenuItem>
{
	public string Action { get; set; }
	public string Controller { get; set; }
	public string Area { get; set; }
	public string Icon { get; set; }
}
```

2. Create JSON file with your navigation menu, for example, `menu.json`:

```
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
        "Controller": "Child_1_1_Controller",
        "Children": [
          {
            "Id": "Child_1_1_1",
            "Title": "Child 1 1 1",
            "Icon": "icon-1-1-1",
            "Action": "Child_1_1_1_Action",
            "Controller": "Child_1_1_1_Controller"
          }
        ]
      },
      {
        "Id": "Child_1_2",
        "Title": "Child 1 2",
        "Icon": "icon-1-2",
        "Action": "Child_1_2_Action",
        "Controller": "Child_1_2_Controller"
      }
    ]
  },
  {
    "Id": "Parent_2",
    "Title": "Parent 2",
    "Icon": "icon-2",
    "Action": "Parent_2_Action",
    "Controller": "Parent_2_Controller",
    "Children": [
      {
        "Id": "Child_2_1",
        "Title": "Child 2 1",
        "Icon": "icon-2-1",
        "Action": "Child_2_1_Action",
        "Controller": "Child_2_1_Controller"
      }
    ]
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

3. Add Navigation to `IServiceCollection` in `Program.cs`:

```
services.AddNavigation()
    .AddJson<MenuItem>("menu.json");
```

4. Use `INavigation<MenuItem>` in View, for example, create `_MenuItem.cshtml` and `_Menu.cshtml`.

`_MenuItem.cshtml`:

```
@model MenuItem

<li data-icon="@Model.Icon" data-id="@Model.Id">
    <a href="@Url.Action(Model.Action, Model.Controller, new { area = Model.Area })" class="@(Model.IsActive ? "active" : "")">@Model.Title</a>
    @if (Model.HasChildren)
    {
        <ul>
            @foreach (var menuItem in Model.Children)
            {
                <partial name="_MenuItem" model="menuItem" />
            }
        </ul>
    }
</li>
```

`_Menu.cshtml`:

```
@using Structr.Navigation
@model INavigation<MenuItem>

<div class="navigation">
    <h4 class="navigation-title">Menu:</h4>
    <ul class="navigation-content">
        @foreach (var menuItem in Model)
        {
            <partial name="_MenuItem" model="menuItem" />
        }
    </ul>
</div>
```

5. Use `_Menu.cshtml` partial in `_Layout.cshtml` or `_Header.cshtml`:

```
@using Structr.Navigation
@inject INavigation<MenuItem> menu

...

<partial name="_Menu" model="menu" />
```

## Usage example in ASP.NET MVC project with XML navigation configuration file

1. Create your navigation item class, for example, `MenuItem`:

```
public class MenuItem : NavigationItem<MenuItem>
{
	public string Action { get; set; }
	public string Controller { get; set; }
	public string Area { get; set; }
	public string Icon { get; set; }
}
```

2. Create XML file with your navigation menu, for example, `menu.xml`:

```
<?xml version="1.0" encoding="utf-8" ?>
<menu>
    <item id="Parent_1" title="Parent 1" Action="Parent_1_Action" Controller="Parent_1_Controller" icon="icon-1">
        <item id="Child_1_1" title="Child 1 1" Action="Child_1_1_Action" Controller="Child_1_1_Controller" icon="icon-1-1">
            <item id="Child_1_1_1" title="Child 1 1 1" Action="Child_1_1_1_Action" Controller="Child_1_1_1_Controller" icon="icon-1-1-1"/>
        </item>
        <item id="Child_1_2" title="Child 1 2" Action="Child_1_2_Action" Controller="Child_1_2_Controller" icon="icon-1-2"/>
    </item>
    <item id="Parent_2" title="Parent 2" Action="Parent_2_Action" Controller="Parent_2_Controller" icon="icon-2">
        <item id="Child_2_1" title="Child 2 1" Action="Child_2_1_Action" Controller="Child_2_1_Controller" icon="icon-2-1"/>
    </item>
    <item id="Parent_3" title="Parent 3" Action="Parent_3_Action" Controller="Parent_3_Controller" icon="icon-3"/>
</menu>
```

3. Add Navigation to `IServiceCollection` in `Program.cs`:

```
services.AddNavigation()
    .AddXml<MenuItem>("menu.xml");
```

4. Use `INavigation<MenuItem>` in View, for example, create `_MenuItem.cshtml` and `_Menu.cshtml`.

`_MenuItem.cshtml`:

```
@model MenuItem

<li data-icon="@Model.Icon" data-id="@Model.Id">
    <a href="@Url.Action(Model.Action, Model.Controller, new { area = Model.Area })" class="@(Model.IsActive ? "active" : "")">@Model.Title</a>
    @if (Model.HasChildren)
    {
        <ul>
            @foreach (var menuItem in Model.Children)
            {
                <partial name="_MenuItem" model="menuItem" />
            }
        </ul>
    }
</li>
```

`_Menu.cshtml`:

```
@using Structr.Navigation
@model INavigation<MenuItem>

<div class="navigation">
    <h4 class="navigation-title">Menu:</h4>
    <ul class="navigation-content">
        @foreach (var menuItem in Model)
        {
            <partial name="_MenuItem" model="menuItem" />
        }
    </ul>
</div>
```

5. Use `_Menu.cshtml` partial in `_Layout.cshtml` or `_Header.cshtml`:

```
@using Structr.Navigation
@inject INavigation<MenuItem> menu

...

<partial name="_Menu" model="menu" />
```

## Breadcrumbs usage example in ASP.NET MVC project

1. Create your breadcrumb class, for example, `Breadcrumb`:

```
public class Breadcrumb : NavigationItem<Breadcrumb>
{
    public string Action { get; set; }
    public string Controller { get; set; }
    public string Area { get; set; }
}
```

2. Create JSON or XML file with your breadcrumbs, for example, `breadcrumbs.json` or `breadcrumbs.xml` similarly `menu.json` or `menu.xml`.

3. Add Navigation to `IServiceCollection` in `Program.cs`:

```
services.AddNavigation()
    .AddJson<Breadcrumb>("breadcrumbs.json");
```

or

```
services.AddNavigation()
    .AddXml<Breadcrumb>("breadcrumbs.xml");
```

4. Use `IBreadcrumbNavigation<Breadcrumb>` in View, for example, create `_Breadcrumbs.cshtml`:

```
@using Structr.Navigation
@model IBreadcrumbNavigation<Breadcrumb>

<div class="navigation breadcrumbs">
    <h4 class="navigation-title">Breadcrumbs:</h4>
    <ul class="navigation-content">
        @foreach (var breadcrumb in Model)
        {
            <li>
                @if (breadcrumb.IsActive)
                {
                    @breadcrumb.Title
                }
                else
                {
                    <a href="@Url.Action(breadcrumb.Action, breadcrumb.Controller, new { area = breadcrumb.Area })">@breadcrumb.Title</a>
                }
            </li>
        }
    </ul>
</div>
```

5. Use `_Breadcrumbs.cshtml` partial in `_Layout.cshtml`:

```
@using Structr.Navigation
@inject IBreadcrumbNavigation<Breadcrumb> breadcrumbs

...

<partial name="_Breadcrumbs" model="breadcrumbs" />
```

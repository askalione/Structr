# Menu

Example of common menu item:

```csharp
public class MenuItem : NavigationItem<MenuItem>
{
	public string Action { get; set; }
	public string Controller { get; set; }
	public string Area { get; set; }
	public string Icon { get; set; }
}
```

`INavigation<MenuItem>` is the main service to get navigation. `INavigation<MenuItem>` created once per request within the scope.

For example, create `_MenuItem.cshtml` and `_Menu.cshtml` views.

`_MenuItem.cshtml`:

```html+razor
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

```html+razor
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

Then you can inject `INavigation<MenuItem>`into `_Layout.cshtml` and use `_Menu.cshtml` partial view to rendering menu.

`_Layout.cshtml`:

```html+razor
@using Structr.Navigation
@inject INavigation<MenuItem> menu

<partial name="_Menu" model="menu" />
```
# Breadcrumbs

Example of common breadcrumb item:

```csharp
public class Breadcrumb : NavigationItem<Breadcrumb>
{
    public string Action { get; set; }
    public string Controller { get; set; }
    public string Area { get; set; }
}
```

`IBreadcrumbNavigation<Breadcrumb>` is the main service to get navigation. `IBreadcrumbNavigation<Breadcrumb>` created once per request within the scope.

For example, create `_Breadcrumbs.cshtml` view:

```html+razor
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

Then you can inject `IBreadcrumbNavigation<Breadcrumb>` into `_Layout.cshtml` and use `_Breadcrumbs.cshtml` partial view to rendering breadcrumbs.

`_Layout.cshtml`:

```html+razor
@using Structr.Navigation
@inject IBreadcrumbNavigation<Breadcrumb> breadcrumbs

<partial name="_Breadcrumbs" model="breadcrumbs" />
```
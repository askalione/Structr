# AspNetCore TagHelpers

This part contains number of tag-helpers that could be used in many different situations.

## Contents

* [AnchorMatchTagHelper](#anchormatchtaghelper.md) - add needed css-classes to target anchor depending on current url;
* [AppendClassTagHelper](#appendclasstaghelper) - add needed css-classes to target element dynamically regarding provided condition;
* [PageInfoTagHelper](#pageinfotaghelper) - organize info-text about items lists pagination;
* [PageSizeTagHelper](#pagesizetaghelper) - create dropdown with possible number of pages on list;
* [PaginationTagHelper](#paginationtaghelper) - create pagination controls;
* [SortTagHelper](#sorttaghelper) - organize items list sorting;
* [TagHelperOutputExtensions](#taghelperoutputextensions) - some extension methods for `TagHelperOutput` class.

## AnchorMatchTagHelper

An `AnchorTagHelper` implementation that adds CSS class to target `<a>` element when current Area, Controller and Action `RouteData` values correspond to specified ones.

| Property name | Property type | Description |
| --- | --- | --- |
| MatchClass | `string` | A CSS class to add to html element. |

```html

<a asp-action="Details" asp-route-id="@user.Id" asp-match-class="active" class="nav-link">Details</a>
<a asp-action="Contracts" asp-route-id="@user.Id" asp-match-class="active" class="nav-link">Contracts</a>
<a asp-action="Statistics" asp-route-id="@user.Id" asp-match-class="active" class="nav-link">Statistics</a>

```

Each of links above will be highlighted by adding `active` css class only when user currently located on corresponding page (which means required area, controller and action).

## AppendClassTagHelper

A `TagHelper` implementation that adds CSS class to target element when "asp-append-if" is set to `true`.

| Property name | Property type | Description |
| --- | --- | --- |
| Append | `bool` | A condition that determines whenever class should or shouldn't be added. |
| Class | `bool` | A CSS class to add to html element. |

```html

<a asp-action="Index" asp-controller="Departments" asp-append-if='@(controller == "Departments")' asp-append-class="active" class="dropdown-item">Departments list</a>

```

## PageInfoTagHelper

A `TagHelper` implementation allowing to organize info-text about pagination in a simple manner.

| Property name | Property type | Description |
| --- | --- | --- |
| Options | `PageInfoOptions` | Options to be used while creating info-text about pagination. |

The `PageInfoOptions` are:

| Property name | Property type | Description |
| --- | --- | --- |
| PagedList | `IPagedList` | An instance of `Structr.Collections.IPagedList` to get information about pagination from. |
| Format | `string` | Info-text format string. It uses string-interpolation with following 5 parameters: 0 - page number, 1 - total pages, 2 - first item on page, 3 - last item on page, 4 - total items. |

```html

<page-info asp-options='new PageInfoOptions { PagedList = Model.Items, Format = "Page {0} of {1}. Showing items {2} through {3} of {4}." }' />

```

After rendering it will give:

```html

<div class="page-info">Page 7 of 12. Showing items 61 through 70 of 116.</div>

```

## PageSizeTagHelper

A `TagHelper` implementation that creates dropdown menu for page size changing.

| Property name | Property type | Description |
| --- | --- | --- |
| Options | `PageSizeOptions` | Options influencing appearance of dropdown menu. |

The `PageSizeOptions` are:

| Property name | Property type | Description |
| --- | --- | --- |
| AllItemsFormat | `string` | A text to show for menu element corresponding to visualizing of all items. |
| DropdownMenuAlign | `PageSizeDropdownMenuAlign` | Align of dropdown menu elements. |
| ContainerCssClass | `string` | Css class for controls container. |
| DropdownCssClass | `string` | Css class for div container of dropdown defining count of elements on page. |
| DropdownToggleCssClass | `string` | Css class for button of dropdown defining count of elements on page. |
| DropdownToggleAttribute | `string` | Name of html-attribute to identify a dropdown. |
| ItemsPerPage | `IEnumerable<int>` | List of possible values of page sizes. |
| PageSizeRouteParamName | `string` | Name of route parameter containing the page size value. |
| DefaultPageSize | `int` | Page size by default. |

```html

<page-size asp-options='@(new PageSizeOptions { ItemsPerPage = Model.Query.ItemsPerPage, DefaultPageSize = Model.Query.PageSize, AllItemsFormat = "Show all", DropdownToggleCssClass = "btn btn-outline-secondary bd bd-grey-300" })' />

```

After rendering it will give:

```html

<div class="page-size dropdown dropup">
    <button class="btn btn-outline-secondary bd bd-grey-300 dropdown-toggle" data-toggle="dropdown" type="button">25</button>
    <div class="dropdown-menu-right dropdown-menu">
        <a class="dropdown-item" href="/ru/admin/bundles/?pagesize=25">25</a>
        <a class="dropdown-item" href="/ru/admin/bundles/?pagesize=50">50</a>
        <a class="dropdown-item" href="/ru/admin/bundles/?pagesize=100">100</a>
    </div>
</div>

```

## PaginationTagHelper

A `TagHelper` implementation creating an array of buttons and other elements forming UI pagination controls without boring manual work.

| Property name | Property type | Description |
| --- | --- | --- |
| Options | `PaginationOptions` | Options influencing appearance of UI pagination controls. |

The `PaginationOptions` are:

| Property name | Property type | Description |
| --- | --- | --- |
| PagedList | `IPagedList` | An instance of `Structr.Collections.IPagedList` to get information about pagination from. |
| PageUrlGenerator | `Func<int, string>` | Factory intended for generation of urls to different pages. |
| Display | `PaginationDisplayMode` | Determines display mode for all pagination controls. |
| DisplayLinkToFirstPage, etc. | `PaginationDisplayMode` | Determines display mode for different buttons. |
| DisplayLinkToIndividualPages | `bool` | Determines whenever buttons to specific pages should be displayed or not. Default value is `true`. |
| MaximumPageNumbersToDisplay | `bool` | Maximum count of buttons with page numbers to display. Default value is 3. |
| DisplayEllipsesWhenNotShowingAllPageNumbers | `bool` | Determines whenever to display ellipses between sets of buttons or not. |
| EllipsesFormat | `string` | Format of ellipsis displayed between sets of page buttons.  Default value is `...`. |
| LinkToFirstPageFormat, etc. | `string` | Format of buttons redirecting to first and other pages respectively. Default values are `««`, `«`, etc. |
| FunctionToDisplayEachPageNumber | `Func<int, string>` | Allows to transform output of page numbers allowing to build for example something like "First", "Second", etc. |
| DelimiterBetweenPageNumbers | `string` | Delimiter strings to place between page numbers. |
| FunctionToTransformEachPageLink | `string` | Method allowing to format HTML for `li` and `a` inside of paging controls, using TagBuilders. |
| PageNumberRouteParamName | `string` | Route parameter name for page number. Default value is `page`. |

```html

<pagination asp-options='@(new PaginationOptions {
    PagedList = Model.Items,
    UlElementCssClasses = new[] { "pagination", "pagination-basic", "pagination-primary", "mg-b-0" },
    LinkToFirstPageFormat = "«",
    LinkToLastPageFormat = "»"
})' />

```

After rendering it will give:

```html

<div class="pagination-container">
    <ul class="mg-b-0 pagination-primary pagination-basic pagination">
        <li class="page-item first pagination-first"><a class="page-link" href="/admin/users/?page=1">«</a></li>
        <li class="page-item disabled pagination-ellipses"><a class="page-link">…</a></li>
        <li class="page-item"><a class="page-link" href="/admin/users/?page=3">3</a></li>
        <li class="page-item active"><a class="page-link">4</a></li>
        <li class="page-item"><a class="page-link" href="/admin/users/?page=5">5</a></li>
        <li class="page-item disabled pagination-ellipses"><a class="page-link">…</a></li>
        <li class="page-item last pagination-last"><a class="page-link" href="/admin/users/?page=256">»</a></li>
    </ul>
</div>

```

## SortTagHelper

A `TagHelper` implementation creating sorting controls for UI.

| Property name | Property type | Description |
| --- | --- | --- |
| Sort | `string` | Name of Model field to sort by. When value of field equals `DefaultSort` then sorting will be performed. |
| DefaultSort | `string` | Value of field to sort by, taken from query (url). For example `@Model.Query.Sort`. |
| DefaultOrder | `SortOrder?` | Sort order to be used, taken from query (url). For example `@Model.Query.Order`. |
| Options | `SortOptions` | Options influencing appearance of sorting controls. |

The `SortOptions` are:

| Property name | Property type | Description |
| --- | --- | --- |
| LinkCssCLass | `string` | Css class for links. Default value is `sort`. |
| ActiveCssClass | `string` | Css class for active element. Default value is `active`. |
| SortRouteParamName | `string` | Name of route parameter containing name of field to sort by. Default value is `sort`. |
| OrderRouteParamName | `string` | Route parameter name for sort order. Default value is `order`. |

```html

<li class="list-sort-item list-inline-item"
    asp-sort="Year"
    asp-default-sort="@Model.Query.Sort"
    asp-default-order="@Model.Query.Order">
    Sort by year
</li>

```

After rendering it will give:

```html

<li class="list-sort-item list-inline-item">
    <a class="sort" href="/Books/?sort=Year&order=Asc">Sort by year</a>
</li>

```

## TagHelperOutputExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| AddClass | `void` | Adds specified class to TagHelper output using using built-in instance of the `System.Text.Encodings.Web.HtmlEncoder`. |
| RemoveClass | `void` | Removes specified class from TagHelper output using built-in instance of the `System.Text.Encodings.Web.HtmlEncoder`. |
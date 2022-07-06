# AspNetCore

**Structr.AspNetCore** package contains number of classes (including tag-helpers) and extension methods to supply wide range of developer basic needs in different situations of web-development.

Big part of package consists of extensions for controllers, HttpContext, etc., allowing to avoid redundant code in widespread cases.

## Installation

AspNetCore package is available on [NuGet](https://www.nuget.org/packages/Structr.AspNetCore/). 

```
dotnet add package Structr.AspNetCore
```

## Setup

There are different ways to configure Structr.AspNetCore services and most common of them is adding: 

```csharp
services.AddAspNetCore();
```

This will add all tools described in [contents](#contents) section below.
But if you need only some of tools then these extension methods for IServiceCollection could be used:

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientAlerts | `IServiceCollection` | Add services assisting in transferring alerts from server side to client. |
| AddClientOptions | `IServiceCollection` | Add services assisting in passing data represented by dictionary via `Microsoft.AspNetCore.Http.HttpContext.Items` |
| AddActionContextAccessor | `IServiceCollection` | Add [`IActionContextAccessor`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.infrastructure.iactioncontextaccessor?view=aspnetcore-6.0) service. |
| AddUrlHelper | `IServiceCollection` | Add [`Microsoft.AspNetCore.Mvc.IUrlHelper`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iurlhelper?view=aspnetcore-6.0) service. |

## Contents

* [Client](Client.md) - methods providing functionality for interacting with app's client-side;
* [Http](Http.md) - extension methods for http-related stuff such as HttpContext and HttpRequest;
* [JavaScript](JavaScript.md) - provides IActionResults and extension methods related to JavaScript and client-side things;
* [Json](Json.md) - JSON-related controller extensions and actions results;
* [Mvc](Mvc.md) - methods for working with ViewEngine and other common MVC stuff;
* [Referrer](Referrer.md) - provides tools for working with HTTP referer and other related things;
* [Rewrite](Rewrite.md) - extensions and IRule implementations related to modifying url;
* [Routing](Routing.md) - extensions and classes for working with routing;
* [TagHelpers](TagHelpers.md).
# AspNetCore

**Structr.AspNetCore** package contains number of classes (including tag-helpers) and extension methods to supply wide range of developer basic needs in different situations of web-development.

Big part of package consists of extensions for controllers, HttpContext, etc., allowing to avoid redundant code in widespread cases.

## Installation

Abstractions package is available on [NuGet](https://www.nuget.org/packages/Structr.AspNetCore/). 

```
dotnet add package Structr.AspNetCore
```

## Contents

* [HttpContextExtensions](AspNetCore#HttpContextExtensions.md)
* [JavaScript](AspNetCore-JavaScript.md)
* [Mvc](AspNetCore-Mvc.md)
* [Rewrite](AspNetCore#Rewrite.md)
* [Routing](AspNetCore-Routing.md)
* [TagHelpers](AspNetCore-TagHelpers.md)

## HttpContextExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| GetIpAddress | `string` | Gets IP-address common human-readable representation for remote target.
| GetAuthenticationSchemesAsync | `Task<IEnumerable<AuthenticationScheme>>` | Gets available authentication schemes.
| IsSupportedAuthenticationSchemeAsync | `bool` | Determines whenever specified authentication scheme is available in current context.

## Rewrite

This section contains rewrite rules (implementing `IRule`) and extensions for `RewriteOptions` that could help in managing request routings and redirects.

| Rule name | Description | Example |
| --- | --- | --- |
| RedirectToLowercaseRule | Rule performing redirect for GET requests to lower case url in case any upper characters are present | http://localhost:5001/Home/Index => http://localhost:5001/home/index |
| RedirectToTrailingSlashRule | Rule performing redirect for GET requests by adding a trailing slash. | http://localhost:5001/Home/Index?search=hello => http://localhost:5001/Home/Index/?search=hello |
| RedirectToLowercaseTrailingSlashRule | Rule performing redirect for GET requests to lower case url in case any upper characters are present, while adding trailing slash. | http://localhost:5001/Home/Index?search=hello => http://localhost:5001/home/index/?search=hello |

Corresponding extensions are:

| Method name | Return type | Description |
| --- | --- | --- |
| AddRedirectToLowercase | `RewriteOptions` | Define rule performing redirect with status 301 for GET requests to lower case url in case any upper characters are present. |
| AddRedirectToTrailingSlash | `RewriteOptions` | Define rule performing redirect with status 301 for GET requests by adding a trailing slash. |
| AddRedirectToLowercaseTrailingSlash | `RewriteOptions` | Define rule performing redirect with status 301 for GET requests to lower case url in case any upper characters are present, while adding trailing slash. |
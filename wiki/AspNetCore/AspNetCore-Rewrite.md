# AspNetCore Rewrite

This section contains rewrite rules (implementing `IRule`) and extensions for `RewriteOptions` that could help in managing request routings and redirects.

| Rule name | Description | Example |
| --- | --- | --- |
| RedirectToLowercaseRule | Rule performing redirect for GET requests to lower case url in case any upper characters are present | http://localhost:5001/Home/Index => http://localhost:5001/home/index |
| RedirectToTrailingSlashRule | Rule performing redirect for GET requests by adding a trailing slash. | http://localhost:5001/Home/Index?search=hello => http://localhost:5001/Home/Index/?search=hello |
| RedirectToLowercaseTrailingSlashRule | Rule performing redirect for GET requests to lower case url in case any upper characters are present, while adding trailing slash. | http://localhost:5001/Home/Index?search=hello => http://localhost:5001/home/index/?search=hello |

Corresponding extensions are:

| Method name | Return type | Description |
| --- | --- | --- |
| AddRedirectToLowercase | `RewriteOptions` | Defines rule performing redirect with status 301 for GET requests to lower case url in case any upper characters are present. |
| AddRedirectToTrailingSlash | `RewriteOptions` | Define rule performing redirect with status 301 for GET requests by adding a trailing slash. |
| AddRedirectToLowercaseTrailingSlash | `RewriteOptions` | Define rule performing redirect with status 301 for GET requests to lower case url in case any upper characters are present, while adding trailing slash. |
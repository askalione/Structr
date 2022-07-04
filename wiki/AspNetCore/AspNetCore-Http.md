# AspNetCore Http

This part provides extension methods for http-related stuff such as HttpContext and HttpRequest.

## HttpContext extensions

| Method name | Return type | Description |
| --- | --- | --- |
| GetIpAddress | `string` | Gets IP-address common human-readable representation for remote target. |
| GetAuthenticationSchemesAsync | `Task<IEnumerable<AuthenticationScheme>>` | Gets available authentication schemes. |
| IsSupportedAuthenticationSchemeAsync | `Task<bool>` | Determines whenever specified authentication scheme is available in current context. |

## HttpRequest extensions

| Method name | Return type | Description |
| --- | --- | --- |
| IsAjaxRequest | `bool` | Determines if request has ajax nature. |
| GetAbsoluteUri | `string` | Gets an absolute Uri of request, combined from scheme, host name, port, path and query. |

## QueryCollection extensions

| Method name | Return type | Description |
| --- | --- | --- |
| ToRouteValueDictionary | `RouteValueDictionary` | Creates an instance of `RouteValueDictionary` containing key-value pairs form specified `IQueryCollection` and appends new value with specified key. |
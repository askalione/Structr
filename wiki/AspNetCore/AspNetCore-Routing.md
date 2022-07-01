# AspNetCore Referrer

This part contains extensions and classes for working with routing.

## SlugifyParameterTransformer

`SlugifyParameterTransformer` is an implementation of IOutboundParameterTransformer that slugifies URL string to make it more user-friendly, for example: `/Users/AccountInfo` => `/Users/Account-Info`.

## RoutingMvcOptionsExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| AddSlugifyRouting | `MvcOptions` | Adds route convention with `SlugifyParameterTransformer`. |

## RoutingRouteOptionsExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| AddSlugifyRouting | `RouteOptions` | Adds `SlugifyParameterTransformer` to constraint map. |
# AspNetCore Referrer

This part contains extensions and classes for working with routing.

## SlugifyParameterTransformer

`SlugifyParameterTransformer` is an implementation of IOutboundParameterTransformer that slugifies URL string to make it more user-friendly, for example: `/Users/AccountInfo` => `/Users/Account-Info`.

## MvcOptions extensions

| Method name | Return type | Description |
| --- | --- | --- |
| AddSlugifyRouting | `MvcOptions` | Adds route convention with `SlugifyParameterTransformer`. |

## RouteOptions extensions

| Method name | Return type | Description |
| --- | --- | --- |
| AddSlugifyRouting | `RouteOptions` | Adds `SlugifyParameterTransformer` to constraint map. |
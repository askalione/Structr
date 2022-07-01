# AspNetCore Client

This contains methods and classes providing functionality for interacting with app's client-side and divided into two categories:

* Client alerts;
* Options;

## Client alerts

Classes and methods intended to help in communicating with client side by sending alerts and messages ti be displayed in UI

### ClientAlert

`ClientAlert` class itself represents an alert of specified type with message. This is a main class of alerts infrastructure.

It has two properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Type | `string` | Type of alert. |
| Message | `string` | Message sending with alert. |

### ClientAlertProvider

`ClientAlertProvider` allows to perform alert-related operations, provides methods for assisting in transferring alerts from server side to client.

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientAlert | `void` | Adds alert to be transferred to client. |
| GetAllClientAlerts | `IEnumerable<ClientAlert>` | Gets alerts transferred to client. |

### Auxiliary entities

`ClientAlertResult` - represents an alert to be send along with action result.

Extension method for `IActionResult`:

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientAlert | `void` | Appends specified alert to `IActionResult` |

### Sample usage

// TODO

## Options

This is a group of methods and classes assisting in passing data represented by dictionary via `HttpContext.Items`.

### ClientOptionProvider

It provides methods assisting in passing data represented by dictionary via `HttpContext.Items`:

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientOptions | `void` | Places set of options with specified key into current `HttpContext`. |
| GetClientOptions | `IReadOnlyDictionary<string, object>` | Gets all options with specified `key` stored in current `HttpContext`. |
| GetAllClientOptions | `IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>>` | Gets all options stored in current `HttpContext`. |
| BuildClientOptionsKey | `string` | Builds a key for options using `routeData` and taking into account current area, controller and action methods. In such way all options will be related to their corresponding pages (urls). |

### ClientOptionControllerExtensions

This controller extensions are helping in manipulating with options:

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientOptions | `void` | Adds client options to context associated with controller. There are different versions taking options as object with some public properties or options dictionary. |

### Sample usage

// TODO
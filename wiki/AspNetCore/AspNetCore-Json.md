# AspNetCore Json

This part provides json-related controller extensions and actions results.

## JsonResponse

`JsonResponse` class represents a result containing data object, errors list, message and success marker. It has following properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Ok | `bool` | `true` when there are no errors in result, otherwise `false`. |
| Message | `string` | A message supplementing the result. Contains text of specified message or first error's message or `null` in case of no errors. |
| Data | `object` | A data attached to result; |
| Errors | [`IEnumerable<JsonResponseError>`](#jsonresponseerror) | A list of errors contained in result; |

The class has different constructors allowing to specify this properties in different combinations.

Working with class is performed by using controller extension [methods](#javascriptcontrollerextensions) that wrap instances of `JsonResponse` with `JsonResult`.

## JsonResponseError

This class represents an error to be transferred to a client.

| Property name | Property type | Description |
| --- | --- | --- |
| Key | `string` | Key corresponding to an error. |
| Message | `string` | Message corresponding to an error. |

## JavaScriptControllerExtensions

These extension methods provide possibilities of sending `JsonResponse` wrapped in `JsonResult`:

| Method name | Return type | Description |
| --- | --- | --- |
| JsonResponse | `JsonResponse` | Creates a `JsonResult` object, with serialized success marker, message and data. |

There are several overloads of this method taking as parameters the success marker, message, errors list and data.
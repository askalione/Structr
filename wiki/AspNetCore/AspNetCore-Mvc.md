# AspNetCore-Mvc

This part provides methods for working with ViewEngine and other common MVC stuff.

## AjaxAttribute

`AjaxAttribute` allows to block non-ajax requests to marked controllers or actions.

## PartialViewControllerExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| RenderPartialViewAsync | `Task<string>` | Renders a partial view using provided model. |
| RenderViewAsync | `Task<string>` | Renders a view using provided model. |

Sample usage is provided below:

```csharp

public async Task<IActionResult> DoSomething(int id, string name)
{
    var model = new MyViewModel { Id = id, Name = name };
    var result = await this.RenderPartialViewAsync("_MyPartialView", model);
    return Content(result); // This returns string with rendered partial view.
}

```

## TempDataDictionaryExtensions

This extension methods help to work with `ITempDataDictionary`:

| Method name | Return type | Description |
| --- | --- | --- |
| Put | `void` | Places a specified object in current `ITempDataDictionary` serializing it. Overwrites an existing value if needed. |
| Peek | `T` | Get object for specified key from current `ITempDataDictionary`. |
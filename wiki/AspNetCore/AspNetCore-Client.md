# AspNetCore Client

This contains methods and classes providing functionality for interacting with app's client-side and divided into two categories:

* [Client alerts](#client-alerts);
* [Client options](#client-options);

## Client alerts

Classes and methods intended to help in communicating with client side by sending alerts and messages ti be displayed in UI

### ClientAlert

`ClientAlert` class itself represents an alert of specified type with message. This is a main class of alerts infrastructure.

It has two properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Type | `string` | Type of alert. E.g. `Success`, `Warning` or `Error`. |
| Message | `string` | Message sending with alert. |

### ClientAlertProvider

`ClientAlertProvider` allows to perform alert-related operations, provides methods for assisting in transferring alerts from server side to client.

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientAlert | `void` | Adds alert to be transferred to client. |
| GetAllClientAlerts | `IEnumerable<ClientAlert>` | Gets all alerts transferred to client. |

### Action result

`ClientAlertResult` - represents an alert to be send along with action result.

Extension method for `IActionResult`:

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientAlert | `void` | Appends specified alert to `IActionResult` |

### Sample usage

Create `IActionResult` extensions:

```csharp
public static class ActionResultExtensions
{
    public static IActionResult Info(this IActionResult result, string message)
        => result.AddClientAlert(new JavaScriptAlert("info", message));

    public static IActionResult Success(this IActionResult result, string message)
        => result.AddClientAlert(new JavaScriptAlert("success", message));

    public static IActionResult Warning(this IActionResult result, string message)
        => result.AddClientAlert(new JavaScriptAlert("warning", message));

    public static IActionResult Error(this IActionResult result, string message)
        => result.AddClientAlert(new JavaScriptAlert("error", message));
}
```

Generate `ClientAlert` in server side:

```csharp
public class UsersController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Edit(int id)
    {
        if (ModelState.IsValid)
        {
            /* Some logic here */
            return RedirectToAction(nameof(Index))
                .Success("Successfully edited.");
        }

        return View(editVm);
    }
}
```

Handle alerts in `TagHelper`:

```csharp
[HtmlTargetElement("client-alerts", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ClientAlertsTagHelper : TagHelper
{
    private readonly IClientAlertProvider _alertProvider;

    public ClientAlertsTagHelper(IClientAlertProvider alertProvider)
        => _alertProvider = alertProvider;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        IEnumerable<ClientAlert> alerts = _alertProvider.GetAllClientAlerts();

        if (alerts.Any() == false)
        {
            output.SuppressOutput();
        }
        else
        {
            output.TagName = "script";
            output.Attributes.SetAttribute("type", "text/javascript");
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Content.AppendHtml(";(function(){");
            foreach (var alert in alerts)
            {
                var type = alert.Type.ToString().ToLower();
                var message = alert.Message.Replace("'", "\'");
                output.Content.AppendHtml($"app.alert('{type}', '{message}');");
            }
            output.Content.AppendHtml("})();");
        }
    }
}
```

Use `ClientAlertsTagHelper` in `_Layout.cshtml` to send `ClientAlert` to client side:

```csharp
<!DOCTYPE html>
<html>
<head>
    /* Some HTML here */
    <client-alerts/>
</body>
</html>
```

## Client options

This is a group of methods and classes assisting in passing data represented by dictionary via `HttpContext.Items`.

### ClientOptionProvider

It provides methods assisting in passing data represented by dictionary via `HttpContext.Items`:

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientOptions | `void` | Places set of options with specified key into current `HttpContext`. |
| GetClientOptions | `IReadOnlyDictionary<string, object>` | Gets all options with specified `key` stored in current `HttpContext`. |
| GetAllClientOptions | `IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>>` | Gets all options stored in current `HttpContext`. |
| BuildClientOptionsKey | `string` | Builds a key for options using `routeData` and taking into account current area, controller and action methods. In such way all options will be related to their corresponding pages (urls). |

### Controller extensions

This controller extensions are helping in manipulating with options:

| Method name | Return type | Description |
| --- | --- | --- |
| AddClientOptions | `void` | Adds client options to context associated with controller. There are different versions taking options as object with some public properties or options dictionary. |

### Sample usage

Generate `ClientOption` in server side:

```csharp
public class UsersController
{
    public async Task<IActionResult> Index()
    {
        /* Some logic here */

        this.AddClientOptions(new
        {
            urlEdit = Url.Action(nameof(Edit))
        });

        return View();
    }
}
```

Handle alerts in `TagHelper`:

```csharp
[HtmlTargetElement("client-options", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ClientOptionsTagHelper : TagHelper
{
    private readonly IClientOptionProvider _optionProvider;

    public ClientOptionsTagHelper(IClientOptionProvider optionProvider)
        => _optionProvider = optionProvider;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var options = _optionProvider.GetAllClientOptions();

        if (options.Any() == false)
        {
            output.SuppressOutput();
        }
        else
        {
            output.TagName = "script";
            output.Attributes.SetAttribute("type", "text/javascript");
            output.TagMode = TagMode.StartTagAndEndTag;

            var serializer = new JsonSerializer()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            output.Content.AppendHtml(";(function(){");
            foreach (var option in options)
            {
                var key = option.Key;
                var keyOptions = JObject.FromObject(option.Value, serializer).ToString(Formatting.None);
                output.Content.AppendHtml($"app.options('{key}', {keyOptions});");
            }
            output.Content.AppendHtml("})();");
        }
    }
}
```

Use `ClientOptionsTagHelper` in `_Layout.cshtml` to send `ClientOption` to client side:

```csharp
<!DOCTYPE html>
<html>
<head>
    /* Some HTML here */
    <client-options/>
</body>
</html>
```
# AspNetCore JavaScript

This part provides IActionResults and extension methods related to JavaScript and client-side things.

## JavaScriptResult

`JavaScriptResult` class serves as a simple wrapper for `ContentResult` with `ContentType` equals `application/javascript` and serves to transfer javascript code to client. It contains no additional fields, properties or logic.

## AjaxRedirectResult

This class represents the implementation of `RedirectResult` that checks that request has ajax nature and performs redirect to Url via ajax instead of normal redirecting. In case of normal request the standard redirecting procedure will be performed.

## JavaScriptControllerExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| JavaScript | `JavaScriptResult` | Creates a `JavaScriptResult` with specified content. |
| AjaxLocalRedirect | `AjaxRedirectResult` | Creates an instance of `AjaxRedirectResult` while checking if url is local. In case of local url the redirect is performed to specified `url`. Otherwise creates response for redirecting to application's root. |
| AjaxRedirect | `AjaxRedirectResult` | Creates an instance of `AjaxRedirectResult` that checks that request has ajax nature and performs redirect to Url via ajax instead of normal redirecting. |

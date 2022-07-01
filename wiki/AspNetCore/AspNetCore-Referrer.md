# AspNetCore Referrer

This part contains tools for working with HTTP referer and other related things. Typical use-case of such tools is, for example, specifying of redirect back to entity's details form from edit form is needed after pressing "Ok" button or "Cancel" button.

## ReferrerControllerExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| RedirectToReferrer | `RedirectResult` | Creates `RedirectResult` object specifying redirect to url depending on presence `__Referrer` key (specified in `ReferrerConstants.Key`) in `HttpRequest.Form`. In case of existing of such key the corresponding url from `From` value will be used to redirect to. In other case the provided `url` parameter will be used. |

## ReferrerHttpRequestExtensions

| Method name | Return type | Description |
| --- | --- | --- |
| GetReferrer | `string` | Gets a string containing url gotten from `HttpRequest.Form` with `__Referrer` key. If there are no such key then the specified url will be returned instead. |

## ReferrerTagHelper

An `TagHelper` implementation adding referrer link to page. It could be helpful when, for example, redirect back to entity's details form from edit form is needed after pressing "Ok" button or "Cancel" button.

```html

<a asp-referrer="@referrerAddress" class="btn btn-link btn-cancel">Cancel</a>

```

This will be (depending on context and app structure) rendered into something like:

```html

<a class="btn btn-link btn-cancel" href="/admin/users/7">Cancel</a>
<input name="__Referrer" type="hidden" value="/admin/users/7"/>

```
# AsyncHelper

`AsyncHelper` is a static class that provides tools to run asynchronous methods in non-async methods.

## RunSync

Synchronously executes provided async method and returns its result.

```csharp
private async Task<int> GetUserNameByIdAsync(int id)
{
    var userName = "";
    /* Some database interaction with use of 'await' */
    return userName;
}

var result = AsyncHelper.RunSync(() => GetUserNameByIdAsync(10)); // will return name of user with id = 10
```

and its void-returning version:

```csharp
private async Task EditUserAsync(int id, string name)
{
    /* Some database interaction with use of 'await' */
}

var result = AsyncHelper.RunSync(() => EditUserAsync(10, "John"));
```
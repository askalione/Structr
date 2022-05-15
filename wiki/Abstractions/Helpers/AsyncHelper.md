# AsyncHelper

`AsyncHelper` is a static class that Provides tools to run asynchronious methods in non-async methods.

## RunSync

Synchroniously executes provided async method and returns it's result.

```csharp
private async Task<int> GetUserNameById(int id)
{
    var userName = "";
    /* Some database interaction with use of 'await' */
    return userName;
}

var result = AsyncHelper.RunSync(() => GetUserNameById(10)); // will return name of user with id = 10
```

and it's void-returning version:

```csharp
private async Task<int> EditUser(int id, string name)
{
    /* Some database interaction with use of 'await' */
}

var result = AsyncHelper.RunSync(() => EditUser(10, "John"));
```
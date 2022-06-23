# PathHelper

`PathHelper` class provides methods for combine and format `ContentDirectory` paths.

## Configure()

Configure `PathOptions` in composition root of your application and then use [Format()](#format) and [Combine()](#combine) methods.

`PathOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Template | `Func<ContentDirectory, string>` | Determines templates for content directories. | 
| Directories | `Dictionary<ContentDirectory, string>` | Determines absolute paths for content directories. | 

Configure `PathOptions` in `Program.cs` file of ASP.NET application:

```csharp
PathHelper.Configure(options =>
{
    options.Template = (directory) => $"|{directory}Directory|";
    options.Directories[ContentDirectory.Base] = "D:\\WebApp";
    options.Directories[ContentDirectory.Data] = "D:\\WebApp\\App_Data";
});
```

## Combine()

After [configuring](#configure) `PathOptions` you can simply combine `ContentDirectory` absolute path and relative path into a common path.

```csharp
string path = PathHelper.Combine(ContentDirectory.Data, "readme.txt"); // Returns "D:\\WebApp\\App_Data\\readme.txt".
```

## Format()

After [configuring](#configure) `PathOptions` you can simply replace content directory templates from path to content directory absolute paths.

```csharp
var path = "|DataDirectory|\\foo\\bar\\baz.txt";
PathHelper.Configure(options =>
{
    options.Template = (directory) => $"|{directory}Directory|";
    options.Directories[ContentDirectory.Base] = "D:\\WebApp";
    options.Directories[ContentDirectory.Data] = "D:\\WebApp\\App_Data";
});

var result = PathHelper.Format(path, ContentDirectory.Data); // Returns "D:\\WebApp\\App_Data\\foo\\bar\\baz.txt".
```
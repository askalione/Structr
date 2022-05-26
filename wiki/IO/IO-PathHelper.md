# PathHelper

Provides static methods for combine and format `ContentDirectory` paths.

## Configure()

Configure `PathOptions`.

```csharp
PathHelper.Configure(options =>
{
    options.Template = (directory) => $"|{directory}Directory|";
    options.Directories[ContentDirectory.Base] = "D:\\Base";
    options.Directories[ContentDirectory.Data] = "D:\\Base\\Data";
});
```

## Combine()

Combines `ContentDirectory` absolute path and relative path into a common path.

```csharp
string path = PathHelper.Combine(ContentDirectory.Data, "readme.txt");
// path == "D:\\Base\\Data\\readme.txt"
```

## Format()

Replace content directory templates from path to content directory absolute paths. For example, replace "|DataCustomDirectory|" to "D:\\Base\\Data":

```csharp
var path = @"|DataCustomDirectory|\foo\bar\baz.txt";
PathHelper.Configure(options =>
{
    options.Template = (directory) => $"|{directory}CustomDirectory|";
    options.Directories[ContentDirectory.Base] = "D:\\Base";
    options.Directories[ContentDirectory.Data] = "D:\\Base\\Data";
});

var result = PathHelper.Format(path, ContentDirectory.Data);
// path == "D:\\Base\\Data\\foo\\bar\\baz.txt"
```
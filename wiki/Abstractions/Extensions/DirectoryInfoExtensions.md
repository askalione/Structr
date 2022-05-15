## GetParent

Helps to find DirectoryInfo for parent n-levels higher than current directory. Nearest parent have level 1, etc.

```csharp
var directoryInfo = new DirectoryInfo(@"d:\dev\sample\1\2\3");
var result = directoryInfo.GetParent(2); // "d:\dev\sample\1"
```
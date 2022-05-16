## ToFileSizeString

Creates a human readable file size string from `long` value.

```csharp
12L.ToFileSizeString(); // ---> 12.0 bytes
2200L.ToFileSizeString(); // ---> 2.1 KB
3330000L.ToFileSizeString(); // ---> 3.2 MB
```
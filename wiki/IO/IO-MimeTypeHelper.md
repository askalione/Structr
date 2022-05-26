# MimeTypeHelper

Provides static methods for getting a file extension (starts with dot, e.g. ".exe") by a MIME type and vice versa.

## GetMimeType()

Returns MIME type by file extension.

```csharp
string mimeType = MimeTypeHelper.GetMimeType(extension);
```

## GetExtension()

Returns file extension by MIME type.

```csharp
string extension = MimeTypeHelper.GetExtension(mimeType);
```

## GetExtensions()

Returns file extensions by MIME type.

```csharp
IEnumerable<string> extensions = MimeTypeHelper.GetExtensions(mimeType);
```
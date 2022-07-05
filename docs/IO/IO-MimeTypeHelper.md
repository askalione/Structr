# MimeTypeHelper

`MimeTypeHelper` class provides methods for getting a file extension (starts with dot, e.g. ".pdf") by a MIME type and vice versa.

## GetMimeType()

Returns MIME type by file extension.

```csharp
string extension = ".pdf";
string mimeType = MimeTypeHelper.GetMimeType(extension); // Returns "application/pdf".
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| extension | `string` | The file extension. |

## GetExtension()

Returns file extension by MIME type.

```csharp
string mimeType = "application/pdf";
string extension = MimeTypeHelper.GetExtension(mimeType); // Returns ".pdf".
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| mimeType | `string` | The MIME type. |
| throwIfNotFound | `bool` | The flag indicates the need for an exception if the MIME type is not found. Default value is `true`. |

## GetExtensions()

Returns file extensions by MIME type. Some MIME types has any various of extensions. For example: 

```csharp
string mimeType = "image/jpeg";
IEnumerable<string> extensions = MimeTypeHelper.GetExtensions(mimeType); // Returns (".jpe", ".jpg", ".jpeg")
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| mimeType | `string` | The MIME type. |
| throwIfNotFound | `bool` | The flag indicates the need for an exception if the MIME type is not found. Default value is `true`. |
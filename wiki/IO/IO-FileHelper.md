# FileHelper

`FileHelper` class provides synchronous and asynchronous methods for the write, read and delete a single file.

## SaveFile()

Synchronously saves a byte array to a file by an absolute path.

```csharp
FileHelper.SaveFile("absolute_file_path", bytes);
```

By default, if the file exists, the `FileHelper` creates a new file with a unique name and returns that name.

```csharp
string existingFilePath = "D:\\readme.txt";
string newFilePath = FileHelper.SaveFile(existingFilePath, bytes); // Returns "D:\\readme_1.txt".
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| path | `string` | The absolute file path to save to. |
| bytes | `byte[]` | The bytes to save to the file. |
| createDirIfNotExists | `bool` | The flag indicates to create destination directory if not exists. Default value is `true`. |
| overrideFileIfExists | `bool` | The flag indicates to override destination file if exists. Default value is `false`. |

## SaveFileAsync()

Asynchronously saves a byte array to a file by an absolute path.

```csharp
await FileHelper.SaveFileAsync("absolute_file_path", bytes);
```

By default, if the file exists, the `FileHelper` creates a new file with a unique name and returns that name.

```csharp
string existingFilePath = "D:\\readme.txt";
string newFilePath = await FileHelper.SaveFileAsync(existingFilePath, bytes); // Returns "D:\\readme_1.txt".
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| path | `string` | The absolute file path to save to. |
| bytes | `byte[]` | The bytes to save to the file. |
| createDirIfNotExists | `bool` | The flag indicates to create destination directory if not exists. Default value is `true`. |
| overrideFileIfExists | `bool` | The flag indicates to override destination file if exists. Default value is `false`. |
| cancellationToken | `CancellationToken` | The token to monitor for cancellation requests. Default value is `None`. |

## ReadFile()

Synchronously reads a file from an absolute path to a byte array.

```csharp
byte[] bytes = FileHelper.ReadFile(filePath);
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| path | `string` | The absolute file path to read to. |
| throwIfNotExists | `bool` | The flag indicates to throw exception if file not exists. Default value is `true`. |

You can also synchronously reads a file from a stream to a byte array.

```csharp
byte[] bytes = FileHelper.ReadFile(stream);
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| stream | `Stream` | The Stream (MemoryStream, FileStream, etc.). |
| initialLength | `long` | Length of returning byte array. Default value is `0`. |

## ReadFileAsync()

Asynchronously reads a file from an absolute path to a byte array.

```csharp
byte[] bytes = await FileHelper.ReadFileAsync(filePath);
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| path | `string` | The absolute file path to read to. |
| throwIfNotExists | `bool` | The flag indicates to throw exception if file not exists. Default value is `true`. |
| cancellationToken | `CancellationToken` | The token to monitor for cancellation requests. Default value is `None`. |

You can also asynchronously reads a file from a stream to a byte array.

```csharp
byte[] bytes = await FileHelper.ReadFileAsync(stream);
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| stream | `Stream` | The Stream (MemoryStream, FileStream, etc.). |
| initialLength | `long` | Length of returning byte array. Default value is `0`. |
| cancellationToken | `CancellationToken` | The token to monitor for cancellation requests. Default value is `None`. |

## DeleteFile()

Deletes a file if it exists.

```csharp
FileHelper.DeleteFile(filePath);
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| path | `string` | The absolute file path to delete to. |

## GetFilePathWithUniqueFileName()

Returns an absolute path with changing destination file name with unique postfix e.g. ("file_1.txt", "file_2.txt").

```csharp
string existingFilePath = "D:\\readme.txt";
string newFilePath = FileHelper.GetFilePathWithUniqueFileName(existingFilePath); // Returns "D:\\readme_1.txt".
```

All parameters:

| Param name | Param type | Description |
| --- | --- | --- |
| path | `string` | The absolute file path. |
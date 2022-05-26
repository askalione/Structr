# FileHelper

`FileHelper` static class provides synchronous and asynchronous methods for the save, read and delete a single file.

# SaveFile()

Synchronously saves a byte array to a file by an absolute path.

```csharp
FileHelper.SaveFile(filePath, bytes);
```

By default, if the file exists, the `FileHelper` creates a new file with a unique name and returns that name.

```csharp
string existingFilePath = "D:\\readme.txt";
string newFilePath = FileHelper.SaveFile(existingFilePath, bytes);
// newFilePath == "D:\\readme_1.txt"
```

All parameters:

| Param | Type |Default value | Comment |
| --- | --- | --- | --- |
| path | string | - | The absolute file path to save to. |
| bytes | byte[] | - | The bytes to save to the file. |
| createDirIfNotExists | bool | true | The flag indicates to create destination directory if not exists. |
| overrideFileIfExists | bool | false | The flag indicates to override destination file if exists. |


# SaveFileAsync()

Asynchronously saves a byte array to a file by an absolute path.

```csharp
await FileHelper.SaveFileAsync(filePath, bytes);
```

By default, if the file exists, the `FileHelper` creates a new file with a unique name and returns that name.

```csharp
string existingFilePath = "D:\\readme.txt";
string newFilePath = await FileHelper.SaveFileAsync(existingFilePath, bytes);
// newFilePath == "D:\\readme_1.txt"
```

All parameters:

| Param | Type |Default value | Comment |
| --- | --- | --- | --- |
| path | string | - | The absolute file path to save to. |
| bytes | byte[] | - | The bytes to save to the file. |
| createDirIfNotExists | bool | true | The flag indicates to create destination directory if not exists. |
| overrideFileIfExists | bool | false | The flag indicates to override destination file if exists. |
| cancellationToken | CancellationToken | None | The token to monitor for cancellation requests. |

# ReadFile()

Synchronously reads a file from an absolute path to a byte array.

```csharp
byte[] bytes = FileHelper.ReadFile(filePath);
```

All parameters:

| Param | Type |Default value | Comment |
| --- | --- | --- | --- |
| path | string | - | The absolute file path to read to. |
| throwIfNotExists | bool| true | The flag indicates to throw exception if file not exists. |

You can also synchronously reads a file from a stream to a byte array.

```csharp
Stream stream;
byte[] bytes = FileHelper.ReadFile(stream);
```

All parameters:

| Param | Type |Default value | Comment |
| --- | --- | --- | --- |
| stream | Stream | - | The Stream (MemoryStream, FileStream, etc.). |
| initialLength | long| 0 | Length of returning byte array. |

# ReadFileAsync()

Asynchronously reads a file from an absolute path to a byte array.

```csharp
byte[] bytes = await FileHelper.ReadFileAsync(filePath);
```

All parameters:

| Param | Type |Default value | Comment |
| --- | --- | --- | --- |
| path | string | - | The absolute file path to read to. |
| throwIfNotExists | bool| true | The flag indicates to throw exception if file not exists. |
| cancellationToken | CancellationToken | None | The token to monitor for cancellation requests. |

You can also asynchronously reads a file from a stream to a byte array.

```csharp
Stream stream;
byte[] bytes = await FileHelper.ReadFileAsync(stream);
```

All parameters:

| Param | Type |Default value | Comment |
| --- | --- | --- | --- |
| stream | Stream | - | The Stream (MemoryStream, FileStream, etc.). |
| initialLength | long| 0 | Length of returning byte array. |
| cancellationToken | CancellationToken | None | The token to monitor for cancellation requests. |

# DeleteFile()

Deletes a file if it exists.

```csharp
FileHelper.DeleteFile(filePath);
```

# GetFilePathWithUniqueFileName()

Returns an absolute path with changing destination file name with unique postfix e.g. ("file_1.exe", "file_2.exe").

```csharp
string existingFilePath = "D:\\readme.txt";
string newFilePath = FileHelper.GetFilePathWithUniqueFileName(existingFilePath);
// newFilePath == "D:\\readme_1.txt"
```
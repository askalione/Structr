# SequentialFileName

Provides static methods for generate new sequntial filename based on guid and time stamp segments using different params, e.g. existing file, MIME type or file extension.

## NewFileName()

Generates new sequntial filename based on guid and time stamp segments.

```csharp
string fileName = SequentialFileName.NewFileName();
// fileName == "20220526232825-b4970420ac3a4370b3b8ff6021f01a4c"
```

Also you can use `NewFileName()` with param `existsFileName` and generate new sequntial filename based on guid and time stamp segments using extension from exists file name.

```csharp
string fileName = SequentialFileName.NewFileName("readme.txt");
// fileName == "20220526232825-b4970420ac3a4370b3b8ff6021f01a4c.txt"
```

## NewFileNameWithMimeType()

Generate new sequntial filename based on guid and time stamp segments with specified MIME type.

```csharp
string fileName = SequentialFileName.NewFileNameWithMimeType("text/xml");
// fileName == "20220526232825-b4970420ac3a4370b3b8ff6021f01a4c.xml"
```

## NewFileNameWithExtension()

Generate new sequntial filename based on guid and time stamp segments with specified extension.

```csharp
string fileName = SequentialFileName.NewFileNameWithExtension(".csv");
// fileName == "20220526232825-b4970420ac3a4370b3b8ff6021f01a4c.csv"
```
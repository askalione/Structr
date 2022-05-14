## AddRangeOverride
Adds new values to source dictionary, overriding values for existing keys.

```csharp
var dictionary = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" },
    { 3, "Three" },
    { 4, "Four" }
};
var newDictionary = new Dictionary<int, string>
{
    { 1, "One_overriden" },
    { 3, "Three_overriden" },
    { 5, "Five_new" }
};
dictionary.AddRangeOverride(newDictionary);

// results in:
//  { 1, "One_overridden" },
//  { 2, "Two" },
//  { 3, "Three_overridden" },
//  { 4, "Four" },
//  { 5, "Five_new" }
```

## AddRangeNewOnly
Adds new values to source dictionary, leaving existing keys values untouched.

```csharp
var dictionary = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" },
    { 3, "Three" },
    { 4, "Four" }
};
var newDictionary = new Dictionary<int, string>
{
    { 1, "One_dont_override" },
    { 3, "Three_dont_override" },
    { 5, "Five_new" }
};
dictionary.AddRangeNewOnly(newDictionary);

// results in:
//  { 1, "One" },
//  { 2, "Two" },
//  { 3, "Three" },
//  { 4, "Four" },
//  { 5, "Five_new" }
```

## AddRange
Adds new values to source dictionary. Throws if one or more keys in new list already exist in source dictionary.

```csharp
var dictionary = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" },
    { 3, "Three" },
    { 4, "Four" }
};
var newDictionary = new Dictionary<int, string>
{
    { 5, "Five_new" }
    { 6, "Six_new" }
};
dictionary.AddRange(newDictionary);

// results in:
//  { 1, "One" },
//  { 2, "Two" },
//  { 3, "Three" },
//  { 4, "Four" },
//  { 5, "Five_new" }
//  { 6, "Six_new" }
```

## ContainsKeys
Checks if **at least one** key from provided list exists in source dictionary.

```csharp
var dictionary = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" },
    { 3, "Three" },
    { 4, "Four" }
};
var result = dictionary.ContainsKeys(new int[] { 1, 2, 3 }); // true
result = dictionary.ContainsKeys(new int[] { 4, 5 }); // true
result = dictionary.ContainsKeys(new int[] { 6, 7 }); // false
```
Structr.Collections package is intended to help organize search results collections into pagination-frendly arrays, which provide all needed data to display page control buttons in UI.
Instance of package main class - **PageList** - contains data about page number, size, first and last pages of whole selection, etc.

# Example
Let's imagine we have some set of goods gotten from database using search criteria "fruits":
```
{"apple", "banana", "pear", "orange", "mandarin", "plum", "lime", "mango", "cherimoya", "feijoa", "guava", "lemon", "kumquat"};
```
But for some strange reason we could show only **3** items from search results per page. So, user will get totally **5** pages and currently will see one of them. Let it be the second one. Than after using all skips and takes in our SQL-query we'll get something like this:
```csharp
var fruits = new List<string> {"orange", "mandarin", "plum"};
```
Before sending this result to presentation level, it will be nice to send some pagination info along with ```fruits```-list to provide data you need for user control generation puproses. These are: buttons with page numbers, arrows, total count info.
At this moment ```PagedList``` comes to our help:
```csharp
var result = new PagedList<int>(fruits, 13, 2, 3);
```
```totalItems``` parameter (which is **13**) usualy comes from first SQL ```COUNT(*)```-like query intended to get total count of elements in search before applying ```SKIP``` and ```TAKE``` operators. The second and third parameters here are: ```pageNumber``` and ```pageSize``` respectively.
Based on such ```result``` you can successfuly create user interface or provide info about search results to you API-client. 
# HierarchyId
`HierarchyId` class provides tools for operating with identificators of objects combined into hierarchical structure. The idea of hierarchical id becomes helpful when working of objects combined into hierarchical structures. It could be organization departments for example.

Instance of `HierarchyId` represents hierarchical identificator for node and contain ids of all ancestors for current node. For example it could be: "/10/38/94/", which means that current node has `id=94`, its parent id is `38` and grandparent id is `10`.

Methods of this class allows getting ancestor of needed level, check if current node is descendant of some other node, move nodes in hierarchy etc.

All methods are described below:

## Methods

| Method name | Return type | Description |
| --- | --- | --- |
| Constructor | `HierarchyId` | Initializes a new instance of the `HierarchyId` class that holds data about provided nodes ids. | 
| GetNode | `int` | Gets last node in current branch of hierarchy |
| IsDescendantOf | `bool` | Determines whether current node is descendant of specified node. |
| GetAncestor | `HierarchyId` | Gets ancestor of current node n-levels higher than itself. |
| GetDescendant | `HierarchyId` | Gets hierarchical id for direct descendant of current node with regular id specified. |
| GetLevel | int | Gets level in hierarchy for current node. |
| Move | - | Moves current node from its current ancestor to new one. |
| ToString | `string` | Creates string representation of current HierarchyId. |
| Parse | `HierarchyId` | Creates `HierarchyId` instance from its string representation. |

## Basic usage

```csharp
// Create hierarchical id from ids of items in brunch of hierarchy
var hierarchyId = new HierarchyId(new int[] { 1, 2, 3, 4, 5 });

// Get string representation
var strId = hierarchyId.ToString(); // "/1/2/3/4/5/"

// Get current node id
var currentId = hierarchyId.GetNode(); // 5

// Check hierarchical relations
var f1 = hierarchyId.IsDescendantOf(3); // true
var f2 = hierarchyId.IsDescendantOf(6); // false

// Get ancestor
var a1 = hierarchyId.GetAncestor(1); // "/1/2/3/4/"
var a2 = hierarchyId.GetAncestor(3); // "/1/2/"

// Get hierarchical id for descendant of current node
var d = hierarchyId.GetDescendant(6); // "/1/2/3/4/5/6/"

// Get level of current node
var level = hierarchyId.GetLevel(); // 5

// Move current node to another brunch
var newHierarchyId = new HierarchyId(new int[] { 11, 12, 13, 14, 15 });
var ancestor1 = new HierarchyId(new int[] { 11, 12, 13 });
var ancestor2 = new HierarchyId(new int[] { 21, 22 });
newHierarchyId.Move(ancestor1, ancestor2); // results in "/21/22/14/15/"
```
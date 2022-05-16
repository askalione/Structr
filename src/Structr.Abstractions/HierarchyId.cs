using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Abstractions
{
    /// <summary>
    /// Provides tools for operating with identificators of objects combined into hierarchical structure.
    /// </summary>
    public class HierarchyId
    {
        private List<int> _nodes;

        private const char _separator = '/';

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyId"> class that holds data about provided nodes ids.
        /// </summary>
        /// <param name="nodes">Hierarchy nodes ids</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HierarchyId(IEnumerable<int> nodes)
        {
            Ensure.NotNull(nodes, nameof(nodes));

            _nodes = nodes.ToList();
        }

        /// <summary>
        /// Gets last node in current branch of hierarchy
        /// </summary>
        /// <returns>Last node id</returns>
        public int GetNode()
            => _nodes.Last();

        /// <summary>
        /// Determines whether current node is descendant of specified node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns><see langword="true"/> if current node is descendant of specified node, overwise <see langword="false"/></returns>
        public bool IsDescendantOf(int node)
            => GetAncestors(1).Contains(node);

        /// <summary>
        /// Gets ancestor of current node <paramref name="n"/> levels higher than itself.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Hierarchical id for ancestor</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public HierarchyId GetAncestor(int n)
        {
            Ensure.GreaterThan(n, 0, nameof(n));

            return new HierarchyId(GetAncestors(n).ToList());
        }

        private IEnumerable<int> GetAncestors(int n)
            => _nodes.Take(_nodes.Count - n); // TODO: add n <= count check

        /// <summary>
        /// Gets hierarchical id for direct descendant of current node with regular id provided in <paramref name="node"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Hierarchical id for direct descendant</returns>
        public HierarchyId GetDescendant(int node)
        {
            var nodes = _nodes.ToList();
            nodes.Add(node);
            return new HierarchyId(nodes);
        }

        /// <summary>
        /// Gets level in hierarchy for current node.
        /// </summary>
        /// <returns>Current node level</returns>
        public int GetLevel()
            => _nodes.Count;

        /// <summary>
        /// Moves current node from it's current ancestor to new one.
        /// </summary>
        /// <param name="sourceAncestor">Source ancestor to move from.</param>
        /// <param name="destAncestor">Destination ancestor to move to.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Move(HierarchyId sourceAncestor, HierarchyId destAncestor)
        {
            Ensure.NotNull(sourceAncestor, nameof(sourceAncestor));
            Ensure.NotNull(destAncestor, nameof(destAncestor));

            var sourceAncestorNode = sourceAncestor.GetNode();
            var sourceAncestorHierarchyIdAsString = sourceAncestor.ToString();
            var currentHierarchyIdAsString = ToString();

            if (IsDescendantOf(sourceAncestorNode) == false)
            {
                throw new InvalidOperationException(
                    $"Current hierarchyid {currentHierarchyIdAsString} isn't descendant of {sourceAncestorHierarchyIdAsString}");
            }

            var result = currentHierarchyIdAsString
                .Replace(sourceAncestorHierarchyIdAsString, destAncestor.ToString());

            _nodes = Parse(result)._nodes.ToList();
        }

        /// <summary>
        /// Creates string representation of current HierarchyId.
        /// </summary>
        /// <returns>String representation of HierarchyId.</returns>
        public override string ToString()
            => ToString(_nodes);

        private string ToString(IEnumerable<int> nodes)
            => _separator + string.Join(_separator.ToString(), nodes) + _separator;

        /// <summary>
        /// Creates <see cref="HierarchyId"/> instance from its string representation.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Instance of <see cref="HierarchyId"/></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static HierarchyId Parse(string value)
        {
            Ensure.NotNull(value, nameof(value));

            var nodes = value.Trim(_separator)
                .Split(_separator)
                .Select(x => int.TryParse(x, out int node) ? node : -1)
                .Where(x => x >= 0)
                .ToList();

            if (nodes.Any() == false)
            {
                throw new InvalidOperationException($"Invalid HierarchyId string value {value}.");
            }

            return new HierarchyId(nodes);
        }
    }
}

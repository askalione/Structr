using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Abstractions
{
    public class HierarchyId
    {
        private List<int> _nodes;

        private const char _separator = '/';

        public HierarchyId(IEnumerable<int> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            _nodes = nodes.ToList();
        }

        public int GetNode()
            => _nodes.Last();

        public bool IsDescendantOf(int node)
            => GetAncestors(1).Contains(node);

        public HierarchyId GetAncestor(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, $"Value is out of range. Value must be greater than 0.");
            }

            return new HierarchyId(GetAncestors(n).ToList());
        }

        private IEnumerable<int> GetAncestors(int n)
            => _nodes.Take(_nodes.Count - n);

        public HierarchyId GetDescendant(int node)
        {
            var nodes = _nodes.ToList();
            nodes.Add(node);
            return new HierarchyId(nodes);
        }

        public int GetLevel()
            => _nodes.Count;

        public void Move(HierarchyId sourceAncestor, HierarchyId destAncestor)
        {
            if (sourceAncestor == null)
            {
                throw new ArgumentNullException(nameof(sourceAncestor));
            }
            if (destAncestor == null)
            {
                throw new ArgumentNullException(nameof(destAncestor));
            }

            var sourceAncestorNode = sourceAncestor.GetNode();
            var sourceAncestorHierarchyIdAsString = sourceAncestor.ToString();
            var currentHierarchyIdAsString = ToString();

            if (IsDescendantOf(sourceAncestorNode) == false)
            {
                throw new InvalidOperationException(
                    $"Current hierarchyid {currentHierarchyIdAsString} is not descendant of {sourceAncestorHierarchyIdAsString}");
            }

            var result = currentHierarchyIdAsString
                .Replace(sourceAncestorHierarchyIdAsString, destAncestor.ToString());

            _nodes = Parse(result)._nodes.ToList();
        }

        public override string ToString()
            => ToString(_nodes);

        private string ToString(IEnumerable<int> nodes)
            => _separator + string.Join(_separator.ToString(), nodes) + _separator;

        public static HierarchyId Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

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

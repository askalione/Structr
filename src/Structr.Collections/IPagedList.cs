using System.Collections.Generic;

namespace Structr.Collections
{
    public interface IPagedList<out T> : IPagedEnumerable, IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }
}

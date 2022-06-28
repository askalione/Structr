using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Represents an operation handler in a filter method.
    /// </summary>
    /// <typeparam name="TResult">The type of result from the handler.</typeparam>
    public delegate Task<TResult> OperationHandlerDelegate<TResult>();
}

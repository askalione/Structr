using System.Threading.Tasks;

namespace Structr.Operations
{
    public delegate Task OperationHandlerDelegate();
    public delegate Task<TResponse> OperationHandlerDelegate<TResponse>();
}

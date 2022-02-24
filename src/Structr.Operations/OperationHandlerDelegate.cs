using System.Threading.Tasks;

namespace Structr.Operations
{
    public delegate Task<TResponse> OperationHandlerDelegate<TResponse>();
}

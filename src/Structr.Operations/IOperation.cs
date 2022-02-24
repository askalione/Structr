namespace Structr.Operations
{
    public interface IOperation<out TResult> : IBaseOperation
    {
    }

    public interface IOperation : IOperation<VoidResult>
    {
    }
}

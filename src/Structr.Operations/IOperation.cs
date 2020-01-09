namespace Structr.Operations
{
    public interface IOperation
    {
    }

    public interface IOperation<out TResult> : IOperation
    {
    }
}

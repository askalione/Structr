namespace Structr.Operations
{
    /// <summary>
    /// Represents an object containing data to perform operation that return result. Usually associated with `Query`.
    /// </summary>
    /// <typeparam name="TResult">Type of result</typeparam>
    public interface IOperation<out TResult> : IBaseOperation
    {
    }

    /// <summary>
    /// Represents an object containing data to perform operation without returning a result. Usually associated with `Command`.
    /// </summary>
    public interface IOperation : IOperation<VoidResult>
    {
    }
}

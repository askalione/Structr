using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils
{
    public interface IStringWriter
    {
        void Write(string? message);

        Task WriteAsync(string? message);
    }
}

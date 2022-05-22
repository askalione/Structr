using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils
{
    public class WriterMock : IStringWriter
    {
        public List<string?> Buffer { get; private set; } = new();
        public int CallCount { get; private set; } = 0;

        public void Write(string? message)
        {
            CallCount++;
            Buffer.Add(message);
        }

        public async Task WriteAsync(string? message)
        {
            CallCount++;
            await Task.Run(() => Buffer.Add(message));
        }

        public static WriterMock New => new WriterMock();
    }
}

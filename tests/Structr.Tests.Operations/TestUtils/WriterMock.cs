using Structr.Tests.Operations.TestUtils.Cqrs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils
{
    public class WriterMock : IStringWriter
    {
        public string Buffer { get; private set; } = "";
        public int CallCount { get; private set; } = 0;

        public void Write(string message)
        {
            CallCount++;
            Buffer += message;
        }

        public async Task WriteAsync(string message)
        {
            CallCount++;
            await Task.Run(() => Buffer += message);
        }

        public static WriterMock New => new WriterMock();
    }
}

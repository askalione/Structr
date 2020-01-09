using System;
using System.IO;
using System.Threading.Tasks;

namespace Structr.Samples.IO
{
    public class StringWriter : IStringWriter
    {
        private readonly TextWriter _writer;

        public StringWriter(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public Task WriteLineAsync(string value)
        {
            return _writer.WriteLineAsync(value);
        }
    }
}

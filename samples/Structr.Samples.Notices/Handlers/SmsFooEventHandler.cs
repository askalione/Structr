using Structr.Notices;
using Structr.Samples.IO;
using Structr.Samples.Notices.Notices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Notices.Handlers
{
    public class SmsFooEventHandler : INoticeHandler<FooEvent>
    {
        private readonly IStringWriter _writer;

        public SmsFooEventHandler(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public Task HandleAsync(FooEvent notice, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync($"Send SMS for identifier {notice.Id}");
        }
    }
}

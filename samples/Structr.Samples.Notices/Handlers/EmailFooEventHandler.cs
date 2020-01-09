using Structr.Notices;
using Structr.Samples.IO;
using Structr.Samples.Notices.Notices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Notices.Handlers
{
    public class EmailFooEventHandler : INoticeHandler<FooEvent>
    {
        private readonly IStringWriter _writer;

        public EmailFooEventHandler(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public Task HandleAsync(FooEvent notice, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync($"Send email for identifier {notice.Id}");
        }
    }
}

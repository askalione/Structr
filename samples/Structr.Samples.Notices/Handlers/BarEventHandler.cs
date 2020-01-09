using Structr.Notices;
using Structr.Samples.IO;
using Structr.Samples.Notices.Notices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Samples.Notices.Handlers
{
    public class BarEventHandler : INoticeHandler<BarEvent>
    {
        private readonly IStringWriter _writer;

        public BarEventHandler(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public Task HandleAsync(BarEvent notice, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync($"Handle `{nameof(BarEvent)}`. Date is `{notice.Date}`");
        }
    }
}

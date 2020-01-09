using Structr.Notices;
using Structr.Samples.IO;
using Structr.Samples.Notices.Notices;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.Notices
{
    public class App : IApp
    {
        private readonly INoticePublisher _publisher;
        private readonly IStringWriter _writer;

        public App(INoticePublisher publisher, IStringWriter writer)
        {
            if (publisher == null)
                throw new ArgumentNullException(nameof(publisher));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _publisher = publisher;
            _writer = writer;
        }

        public async Task RunAsync()
        {
            var fooEvent = new FooEvent { Id = 1 };
            await PublishAsync(fooEvent);

            var barEvent = new BarEvent { Date = new DateTime(2019, 12, 31) };
            await PublishAsync(barEvent);
        }

        private async Task PublishAsync(INotice notice)
        {
            await _publisher.PublishAsync(notice);
            await _writer.WriteLineAsync("----------------");
        }
    }
}

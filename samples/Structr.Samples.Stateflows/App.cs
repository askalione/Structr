using Structr.Samples.IO;
using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Samples.Stateflows.Services;
using Structr.Samples.Stateflows.Stateflows;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows
{
    public class App : IApp
    {
        private readonly IStringWriter _writer;
        private readonly IFooService _fooService;
        private readonly IBarService _barService;

        public App(IStringWriter writer, IFooService fooService, IBarService barService)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (fooService == null)
                throw new ArgumentNullException(nameof(fooService));
            if (barService == null)
                throw new ArgumentNullException(nameof(barService));

            _writer = writer;
            _fooService = fooService;
            _barService = barService;
        }

        public async Task RunAsync()
        {
            var foo = await _fooService.CreateAsync("user@example.com");
            await ExecuteAsync(_fooService.SendAsync(foo.Id));
            await ExecuteAsync(_fooService.AcceptAsync(foo.Id));
            await ExecuteAsync(_fooService.SendAsync(foo.Id));
            await ExecuteAsync(_fooService.DeclineAsync(foo.Id));

            await _writer.WriteLineAsync("-----------------------");

            var bar = await _barService.CreateAsync("Xstmas");
            await ExecuteAsync(_barService.OpenAsync(bar.Id));
            await ExecuteAsync(_barService.ArchiveAsync(bar.Id));
            await ExecuteAsync(_barService.CloseAsync(bar.Id));
            await ExecuteAsync(_barService.OpenAsync(bar.Id));
            await ExecuteAsync(_barService.ArchiveAsync(bar.Id));
        }

        private async Task ExecuteAsync(Task action)
        {
            try
            {
                await action;
                await _writer.WriteLineAsync("OK");
            }
            catch (StateflowException ex)
            {
                await _writer.WriteLineAsync(ex.Message);
            }
        }
    }
}

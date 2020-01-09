using Structr.Samples.IO;
using Structr.Samples.Validation.Models;
using Structr.Validation;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.Validation
{
    public class App : IApp
    {
        private readonly IValidationProvider _provider;
        private readonly IStringWriter _writer;

        public App(IValidationProvider provider, IStringWriter writer)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _provider = provider;
            _writer = writer;
        }

        public async Task RunAsync()
        {
            var bat = new Bat
            {
                Color = EColor.Red,
                Weight = 120,
                Length = 80,
                Height = 200,
                IsRough = true
            };
            await ValidateAsync(bat);

            var baz = new Baz
            {
                Color = EColor.White,
                Weight = 90,
                Height = 250,
                Shape = EShape.Triangle
            };
            await ValidateAsync(baz);
        }

        private async Task ValidateAsync(object instance)
        {
            IValidationResult result = await _provider.ValidateAsync(instance);
            await _writer.WriteLineAsync(result.ToString());
            await _writer.WriteLineAsync("----------------");
        }
    }
}

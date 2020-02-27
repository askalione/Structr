using Structr.Domain;
using Structr.Samples.Domain.BarAggregate;
using Structr.Samples.Domain.BatAggregate;
using Structr.Samples.Domain.BazAggregate;
using Structr.Samples.Domain.FooAggregate;
using Structr.Samples.IO;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.Domain
{
    public class App : IApp
    {
        private readonly IStringWriter _writer;

        public App(IStringWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public async Task RunAsync()
        {
            var foo1 = new Foo(1, "Foo1");
            var foo2 = new Foo(foo1.Id, "Foo2");

            var bar1 = new Bar(Guid.NewGuid(), EBarType.Cold);
            var bar2 = new Bar(Guid.NewGuid(), EBarType.Warm);

            var baz1 = new Baz(EBaz.Primary, "Baz1");
            var baz2 = new Baz(EBaz.Primary, "Baz2");

            var bat1 = new Bat(new BatId(1, 1), "Bat1");
            var bat2 = new Bat(new BatId(1, 2), "Bat2");

            await WriteAsync(foo1, foo2, nameof(foo1), nameof(foo2));
            await WriteAsync(bar1, bar2, nameof(bar1), nameof(bar2));
            await WriteAsync(baz1, baz2, nameof(baz1), nameof(baz2));
            await WriteAsync(bat1, bat2, nameof(bat1), nameof(bat2));
        }

        private async Task WriteAsync<T>(T that, T other, string thatName, string otherName) where T : Entity<T>
        {
            await _writer.WriteLineAsync($"{thatName} {(that == other ? "==" : "!=")} {otherName}");
        }
    }
}

using Structr.Samples.IO;
using Structr.Samples.Specifications.Models;
using Structr.Samples.Specifications.Specs;
using Structr.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structr.Samples.Specifications
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
            List<Foo> items = new List<Foo>
            {
                new Foo { Name = "Abc", Color = Color.White, Age = 10, DateDeleted = null },
                new Foo { Name = "Some name", Color = Color.Black, Age = 5, DateDeleted = DateTime.Now.AddDays(-1) },
                new Foo { Name = "Dfe", Color = Color.Red, Age = 15, DateDeleted = null },
                new Foo { Name = "Creacode", Color = Color.Blue, Age = 25, DateDeleted = DateTime.Now.AddDays(-2) },
                new Foo { Name = "Excalibur", Color = Color.White, Age = 30, DateDeleted = null },
            };

            await WriteAsync(items, new FooIsNotDeletedSpec(), nameof(FooIsNotDeletedSpec));
            await WriteAsync(items, new FooNameContainsTextSpec("c"), nameof(FooNameContainsTextSpec));
            await WriteAsync(items, new FooIsLightAndYoungerSpec(28), nameof(FooIsLightAndYoungerSpec));
        }

        private async Task WriteAsync(IEnumerable<Foo> items, Specification<Foo> spec, string specName)
        {
            await _writer.WriteLineAsync($"---------{specName}---------");
            foreach (var item in items.Where(x => spec.IsSatisfiedBy(x)))
            {
                await _writer.WriteLineAsync(item.ToString());
            }
        }
    }
}

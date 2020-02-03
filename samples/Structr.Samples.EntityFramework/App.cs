using Structr.Abstractions.Extensions;
using Structr.EntityFramework;
using Structr.Samples.EntityFramework.DataAccess;
using Structr.Samples.EntityFrameworkCore.Domain.FooAggregate;
using Structr.Samples.IO;
using Structr.SqlServer;
using System;
using System.Threading.Tasks;

namespace Structr.Samples.EntityFramework
{
    public class App : IApp
    {
        private readonly DataContext _dataContext;
        private readonly IStringWriter _writer;

        public App(DataContext dataContext, IStringWriter writer)
        {
            if (dataContext == null)
                throw new ArgumentNullException(nameof(dataContext));
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            _dataContext = dataContext;
            _writer = writer;
        }

        public async Task RunAsync()
        {
            var connectionString = _dataContext.Database.Connection.ConnectionString;
            Database.EnsureDeleted(connectionString);

            var foo = new Foo(
                EFooType.Smooth,
                new FooDetail
                {
                    Name = "Foo-Name",
                    Description = "Foo-Description"
                }
            );

            foo.Items.Add(new FooItem("FooItem-Name"));

            _dataContext.Foos.Add(foo);

            await _dataContext.SaveChangesAsync();

            await WriteAsync("Added", foo);

            await Task.Delay(10000);

            foo.Detail.Name = "Modified-Name";
            
            await _dataContext.SaveChangesAsync();

            await WriteAsync("Modified", foo);

            await Task.Delay(10000);

            _dataContext.Foos.Remove(foo);

            await _dataContext.SaveChangesAsync();

            await WriteAsync("Deleted", foo);

            Console.ReadKey();
        }

        private async Task WriteAsync(string title, Foo foo)
        {
            await _writer.WriteLineAsync($"-------- {title} --------");
            await _writer.WriteLineAsync(foo.Dump());
        }
    }
}

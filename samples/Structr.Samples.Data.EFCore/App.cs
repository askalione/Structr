using Microsoft.EntityFrameworkCore;
using Structr.Abstractions.Extensions;
using Structr.Data.SqlServer;
using Structr.Samples.Data.EFCore.DataAccess;
using Structr.Samples.Data.EFCore.Domain.FooAggregate;
using Structr.Samples.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Samples.Data.EFCore
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
            var connectionString = _dataContext.Database.GetDbConnection().ConnectionString;
            Database.EnsureDeleted(connectionString);
            Database.EnsureCreated(connectionString);
            await _dataContext.Database.EnsureCreatedAsync();

            var foo = new Foo(EFooType.Smooth, new FooDetail { Name = "Foo-Name", Description = "Foo-Description" });

            foo.Items.Add(new FooItem("FooItem-Name"));

            _dataContext.Foos.Add(foo);
            await _dataContext.SaveChangesAsync();

            await WriteAsync("Added", foo);

            await Task.Delay(10000);

            foo.Detail.Name = "Modified-Name";
            _dataContext.Foos.Update(foo);
            await _dataContext.SaveChangesAsync();

            await WriteAsync("Modified", foo);

            await Task.Delay(10000);

            _dataContext.Foos.Remove(foo);
            await _dataContext.SaveChangesAsync();

            await WriteAsync("Deleted", foo);
        }

        private async Task WriteAsync(string title, Foo foo)
        {
            await _writer.WriteLineAsync($"-------- {title} --------");
            await _writer.WriteLineAsync(foo.Dump());
        }
    }
}

using Structr.Samples.EntityFramework.DataAccess;
using Structr.Samples.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task RunAsync()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}

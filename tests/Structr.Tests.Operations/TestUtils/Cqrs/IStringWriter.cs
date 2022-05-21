using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Operations.TestUtils.Cqrs
{
    public interface IStringWriter
    {
        void Write(string message);

        Task WriteAsync(string message);
    }
}

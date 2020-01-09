using Structr.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Operations.Commands.Baz
{
    public class BazCommandHandler : OperationHandler<BazCommand, DateTime>
    {
        protected override DateTime Handle(BazCommand operation)
        {
            return DateTime.Now;
        }
    }
}

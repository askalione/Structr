using Structr.Operations;
using System;

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

using Structr.Operations;

namespace Structr.Samples.Operations.Commands.Bar
{
    public class BarCommand : IOperation<string>
    {
        public string Name { get; set; }
    }
}

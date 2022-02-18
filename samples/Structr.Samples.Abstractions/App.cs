using Structr.Abstractions.Helpers;
using Structr.Samples.Abstractions.Enum;
using System.Threading.Tasks;

namespace Structr.Samples.Abstractions
{
    public class App : IApp
    {
        public Task RunAsync()
        {
            Run();
            return Task.CompletedTask;
        }

        public void Run()
        {
            // Enum binding
            var flowers = BindHelper.Bind<Flower, EFlower>((o, e) => o.Name = e.ToString());

        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Structr.Operations;
using Structr.Samples.Operations.Decorators;
using System.Threading.Tasks;

namespace Structr.Samples.Operations
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();

            // Add operations
            services.AddOperations(typeof(Program).Assembly);

            // Add decorators
            services.AddTransient(typeof(ICommandDecorator<>), typeof(CommandDecorator<>));
            services.AddTransient(typeof(ICommandDecorator<,>), typeof(CommandDecorator<,>));
            services.AddTransient(typeof(IQueryDecorator<,>), typeof(QueryDecorator<,>));
            //services.Decorate(typeof(IOperationHandler<>), typeof(OperationDecorator<>));
            services.Decorate(typeof(IOperationHandler<,>), typeof(OperationDecorator<,>));

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}

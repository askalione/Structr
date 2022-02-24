using Microsoft.Extensions.DependencyInjection;
using Structr.Operations;
using Structr.Samples.Operations.Decorators;
using Structr.Samples.Operations.Filters;
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

            // Add validation
            services.AddValidation(typeof(Program).Assembly);

            // Add operations
            services.AddOperations(typeof(Program).Assembly);

            // Add operation filters
            services.AddTransient(typeof(IOperationFilter<,>), typeof(CommandValidationFilter<,>));
            services.AddTransient(typeof(IOperationFilter<,>), typeof(QueryLoggingFilter<,>));

            // Add decorators
            services.AddTransient(typeof(ICommandDecorator<,>), typeof(CommandDecorator<,>));
            services.AddTransient(typeof(IQueryDecorator<,>), typeof(QueryDecorator<,>));
            services.Decorate(typeof(IOperationHandler<,>), typeof(OperationDecorator<,>));
                        
            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Structr.Samples.Stateflows.DataAccess;
using Structr.Samples.Stateflows.Domain.BarEntity;
using Structr.Samples.Stateflows.Services;
using Structr.Samples.Stateflows.Stateflows.BarEntity.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structr.Samples.Stateflows
{
    class Program
    {
        static Task Main(string[] args)
        {
            var services = new ServiceCollection();

            // Add sample app
            services.AddSample<App>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IFooService), typeof(FooService));
            services.AddScoped(typeof(IBarService), typeof(BarService));

            // Add stateflows
            services.AddStateflows(typeof(Program).Assembly);

            // Add custom state machine configurations
            services.AddFactory<BarState, IBarStateMachineConfiguration>(new Dictionary<BarState, Type>
            {
                { BarState.Opened, typeof(OpenedBarStateMachineConfiguration) },
                { BarState.Closed, typeof(ClosedBarStateMachineConfiguration) },
                { BarState.Archived, typeof(ArchivedBarStateMachineConfiguration) }
            });

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<IApp>();

            return app.RunAsync();
        }
    }
}

using System;

namespace Structr.Stateflows.Internal
{
    internal static class ConfiguratorProvider
    {
        public static IStateMachineConfigurator<TEntity, TState, TTrigger> GetConfigurator<TEntity, TState, TTrigger>(IServiceProvider serviceProvider)
        {
            var configuratorType = typeof(IStateMachineConfigurator<TEntity, TState, TTrigger>);
            IStateMachineConfigurator<TEntity, TState, TTrigger> configurator;

            try
            {
                configurator = (IStateMachineConfigurator<TEntity, TState, TTrigger>)serviceProvider.GetService(configuratorType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error constructing operation configurator of type {configuratorType}", ex);
            }

            return configurator;
        }
    }
}

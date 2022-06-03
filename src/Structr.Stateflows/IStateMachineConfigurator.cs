namespace Structr.Stateflows
{
    /// <summary>
    /// Specifies statemachine configuration class that determines its behavior including entity's transitions between states.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity which behavior is modeled.</typeparam>
    /// <typeparam name="TState">Type of object describing entity states.</typeparam>
    /// <typeparam name="TTrigger">Type representing set of possible triggers.</typeparam>
    public interface IStateMachineConfigurator<TEntity, TState, TTrigger> : IStateMachineConfiguration<TEntity, TState, TTrigger>
    {
    }
}

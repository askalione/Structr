# Configurations

In more tricky cases it could be useful to separate state machine configurations from one another for different entity states. This could be done by using `IStateMachineConfiguration` instances:

```csharp
public interface IIssueStateMachineConfiguration : IStateMachineConfiguration<Issue, IssueState, IssueAction>
{}

public abstract class IssueStateMachineConfiguration : StateMachineConfiguration, IIssueStateMachineConfiguration
{
    protected readonly ICurrentUserService CurrentUserService;

    protected IssueStateMachineConfigurator(ICurrentUserService currentUserService)
        => CurrentUserService = currentUserService;
}        

// Configuration for IssueState.Draft.
public class DraftIssueStateMachineConfiguration : IssueStateMachineConfiguration
{
    public DraftIssueStateMachineConfiguration(ICurrentUserService currentUserService) : base(currentUserService)
    {}

    protected override void Configure(Stateless.StateMachine<IssueState, IssueAction> stateMachine, Issue entity)
    {
        // Getting current user.
        ICurrentUser currentUser = CurrentUserService.CurrentUser;
        // Getting info about current user for specified issue.
        bool isAdmin = currentUser.HasRole("Admin");
        bool isAuthor = issue.AuthorId == currentUser.Id;
        bool isAssignee = issue.AssigneeId == currentUser.Id;

        stateMachine.Configure(IssueState.Draft)
            .PermitIf(IssueAction.Submit, IssueState.Submitted, () => isAuthor)
            .InternalTransitionIf(IssueAction.Edit, () => isAuthor)
            .InternalTransitionIf(IssueAction.Delete, () => isAuthor);
    }
}

/* ... and other configuration for other states here ...*/
```

That could be really useful when each configuration contains relatively big and unique set of dependencies, so it won't be needed to invoke (inject) any dependence for each state of entity, but decide what configuration and what dependencies do we need in a run-time:

```csharp
// Represents configuration dispatcher.
public class IssueStateMachineConfigurator : IStateMachineConfigurator<Issue, IssueState, IssueAction>
{
    // This factory will allow you to get instance of IIssueStateMachineConfiguration for any specific state.
    private readonly Func<IssueState, IIssueStateMachineConfiguration> _factory;

    public IssueStateMachineConfigurator(Func<IssueState, IIssueStateMachineConfiguration> factory) 
        => _factory = factory;

    public async Task ConfigureAsync(Stateless.StateMachine<IssueState, IssueAction> stateMachine, Issue entity, CancellationToken cancellationToken)
    {
        // Get configuration for this specific state invoking only its dependencies.
        var configuration = _factory(entity.State);
        // Configuring only part of configuration needed for current entity state.
        await configuration.ConfigureAsync(stateMachine, entity, cancellationToken);
    }
}
```

The factory trick above with `Func<IssueState, IIssueStateMachineConfiguration>` could be done easily with Structr.Abstractions package:

```csharp
 services.AddFactory<IssueState, IIssueStateMachineConfiguration>(new Dictionary<IssueState, Type>
{
    { IssueState.Draft, typeof(DraftIssueStateMachineConfiguration) },
    /* ... and other configuration for other states here ...*/
});
```
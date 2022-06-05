# Stateflows

**Structr.Stateflows** package provides functionality for modeling behavior of state-driven entities by creating and using state machines in your .NET application.
It provides simple yet functional API via `Stateflow` object that incapsulate methods and properties to monitor available actions and change state of controlled entity.

---
## Installation

Stateflows package is available on [NuGet](https://www.nuget.org/packages/Structr.Stateflows/). 

```
dotnet add package Structr.Stateflows
```

---
## Basic usage

Let's say we have some application where user could create `Issue` objects containing feedback or comments and publish them. Every such `Issue` has one of these states:

```csharp
public enum IssueState
{
    Draft,
    Submitted,
    InProgress,
    Solved,
    Archived
}
```

Each of such states imply different set of actions, some of which lead to change of state (like Submitting or Solving) and some not (like Editing):

```csharp
public enum IssueAction
{
    Submit,
    Edit,
    TakeIntoWork,
    Solve,
    /*etc.*/
}
```

Any action could be made only in certain states of object and here Structr.Stateflows offers `Stateflow` class which instances will help determining whether you could for example submit your issue or edit it. Stateflow combines both object instance and it's behavior represented by simple `IStateMachine` which could answer our main question - "what we could do with our `Issue` object?". Let's take issue submit command for example:

```csharp
public class IssueService
{
    // Injecting IIssueStateflowProvider which will supply us with Stateflow instances corresponding to our Issue objects.
    private readonly IIssueStateflowProvider _stateflowProvider;
    public IssueService(IIssueStateflowProvider stateflowProvider) => _stateflowProvider = stateflowProvider;

    public async Task SubmitAsync(int id, CancellationToken cancellationToken)
    {
        // Getting Stateflow object for our issue's id.
        var stateflow = await _stateflowProvider.GetStateflowAsync(id, cancellationToken);

        // Getting issue itself containing in Stateflow object.
        var issue = stateflow.Entity;
        
        // Getting state machine.
        var stateMachine = stateflow.StateMachine;

        // Checking whenever we could submit our issue.
        if (stateMachine.CanFire(IssueAction.Submit) == false)
        {                
            throw new StateflowException($"Submit operation is not permitted");
        }

        // Submitting issue while changing its state to "Submitted".
        stateMachine.Fire(IssueAction.Submit);
        
        // Performing of some other actions.
        issue.DateSubmitted = DateTime.Now;

        /* saving changed entity to database */
    }
}
```

All decisions about whenever we could or couldn't submit the issue are made within state machine and provided to us along with the Issue object itself by stateflow object. So in any methods working with Issue objects vital decisions will take only one or two statements leaving sophisticated business logic enclosed in state machine.

Incapsulating all this logic "in one place" will be the best choice in terms of programing design. And `IssueStateMachineConfigurator` will be just that place. From it state machine will get needed decisions:

```csharp
// IStateMachineConfigurator is a Structr.Stateflows interface allowing you to configure Structr state machine.
public class IssueStateMachineConfigurator : IStateMachineConfigurator<Issue, IssueState, IssueAction>
{
    // As long as IssueStateMachineConfigurator is a normal .net class with it's own constructor we could inject into it any dependencies needed.
    // Here we are adding some IUserProvider to be able to take into account info about current user while modeling Issue's behavior for its different states.
    private readonly IUserProvider _userProivder;
    public IssueStateMachineConfigurator(IUserProvider userProvider) => _userProivder = userProvider;

    // By default Structr.Stateflows uses Stateless.StateMachine under the hood but this can be changed if needed.
    public async Task ConfigureAsync(Stateless.StateMachine<IssueState, IssueAction> stateMachine, Issue entity, CancellationToken cancellationToken)
    {
        // Getting current user
        var user = await _userProivder.GetCurrentUserAsync(cancellationToken);

        // While using Stateless.StateMachine we will apply its configuration methods.
        // First configure Draft state allowing to edit or submit Issue only to it's creator. Additionally only Issue with comment could be submitted.
        stateMachine.Configure(IssueState.Draft)
            .InternalTransitionIf(IssueAction.Edit, (x) => user.Id == entity.CreatorId, (t) => { })
            .PermitIf(IssueAction.Submit, IssueState.Submitted, () => string.IsNullOrEmpty(entity.Comment) && user.Id == entity.CreatorId);

        /* ...configuration stuff for other states... */
    }
}
```

`IIssueStateflowProvider` interface and its implementation used in example with issue submitting above are rather simple:

```csharp
// The IStateflowProvider needs to be implemented in order to get Stateflow objects in your app.
public interface IIssueStateflowProvider : IStateflowProvider<Issue, int, IssueState, IssueAction> { }

public class IssueStateflowProvider : IIssueStateflowProvider
{
    private readonly IRepository<Issue> _repository;
    // This one is provided by Stateflow package and will give us an instance of the default state machine implementation.
    private readonly IStateMachineProvider _stateMachineProvider;
    public IssueStateflowProvider(IRepository<Issue> repository, IStateMachineProvider stateMachineProvider)
    {
        _repository = repository;
        _stateMachineProvider = stateMachineProvider;
    }
    public async Task<Stateflow<Issue, IssueState, IssueAction>> GetStateflowAsync(int entityId, CancellationToken cancellationToken)
    {
        // Finding our Issue object via repository.
        var entity = await _repository.GetAsync(entityId);
        // Getting state machine.
        var stateMachine = await _stateMachineProvider.GetStateMachineAsync(entity, cancellationToken);
        // Creating stateflow instance for given entity.
        return new Stateflow<Issue, IssueState, IssueAction>(entity, stateMachine);
    }
}
```

The last step is to add Structr.Stateflows services to your app. This will automatically register all configurator classes that implement `IStateMachineConfigurator` and stateflow providers that implement `IStateflowProvider`. That's it - now `IIssueStateflowProvider` is ready to be injected into your services.

```csharp
var services = new ServiceCollection();
services.AddStateflows(typeof(Program).Assembly);
/* ... */
```

---
## StateMachine

The main "worker" of Structr.Stateflows package is an `IStateMachine` which provide main functionality related to behavior modelling. It specifies properties and methods allowing to determine whenever we could or couldn't perform certain actions depending on specified entity state. These are:

| Property name | Property type | Description |
| --- | --- | --- |
| `State` | TState | Represents current state of entity. `TState` type could variate but in most cases it's a some kind of enumeration. |
| `PermittedTriggers` | IEnumerable<TTrigger> | Set of permitted triggers (or actions) in current state of entity. `TTrigger` is also in most cases an enumeration. |

| Method name | Description |
| --- | --- |
| `CanFire` | Determines whenever specified trigger can be fired in current state. |
| `Fire` | Transition from the current state via the specified trigger. The target state is determined by the configuration of the current state. Actions associated with leaving the current state and entering the new one will be invoked. |

The default implementation of `IStateMachine` provided by Structr.Stateflows uses under the hood the popular state machine package for .NET - [Stateless](https://github.com/dotnet-state-machine/stateless) to perform it's methods. So all configuration process in `IStateMachineConfigurator` implementations (like `IssueStateMachineConfigurator`) should be done taking into account Stateless docs.

---
## Advanced features

### Individual configurations for different states

In more tricky cases it could be useful to separate state machine configurations from one another for different entity states. This could be done by using `IStateMachineConfiguration` instances:

```csharp
public interface IIssueStateMachineConfiguration : IStateMachineConfiguration<Issue, IssueState, IssueAction> { }
public class InProgressIssueStateMachineConfiguration : IIssueStateMachineConfiguration
{
    /*...waaay too many dependencies are injected here...*/
    protected async Task ConfigureAsync(Stateless.StateMachine<IssueState, IssueAction> stateMachine, Issue entity)
    {
        /*...and used here...*/
        stateMachine.Configure(IssueState.InProgress)
            .Permit(IssueAction.Solve, IssueState.Solved)
            .Permit(IssueAction.Close, IssueState.Closed);
    }
}
/* ... and other IIssueStateMachineConfiguration for other states here ...*/
```

That could be really useful when each configuration contains relatively big and unique set of dependencies, so it won't be needed to invoke (inject) any dependence for each state of entity, but decide what configuration and what dependencies do we need in a run-time:

```csharp
public class IssueStateMachineConfigurator : IStateMachineConfigurator<Issue, IssueState, IssueAction>
{
    // This factory will allow you to get instance of IIssueStateMachineConfiguration for any specific state.
    private readonly Func<IssueState, IIssueStateMachineConfiguration> _factory;
    public IssueStateMachineConfigurator(Func<IssueState, IIssueStateMachineConfiguration> factory) => _factory = factory;
    public async Task ConfigureAsync(Stateless.StateMachine<IssueState, IssueAction> stateMachine, Issue entity, CancellationToken cancellationToken)
    {
        // Get configuration for this specific state invoking only its dependencies.
        var configuration = _factory(entity.State);
        // Configuring only part of configuration needed for current entity state.
        await configuration.ConfigureAsync(stateMachine, entity, cancellationToken);
    }
}
```

The factory trick above with `Func<IssueState, IIssueStateMachineConfiguration>` could be done easily with Structr.Abstractions package. See more [here](https://github.com/askalione/Structr/blob/master/src/Structr.Abstractions/ServiceCollectionExtensions.cs).

### Changing state machine implementation

As was said above the default implementation of `IStateMachine` uses Stateless in it's internals and it is self is created by `IStateMachineProvider` default implementation. When for some reason the Stateless in `IStateMachine` is not suitable for your needs, it could be replaced by any other implementation on registration step, using special options:

```csharp
serviceCollection.AddStateflows(x =>
    {
        x.ProviderType = typeof(MyStateMachineProvider);
    },
    this.GetType().Assembly)
```

This would work after you define your own implementation of `IStateMachine` (some `MyStateMachine` for example) which would use some over mechanisms to power `CanFire` and other methods and properties of `IStateMachine`. After that just create `MyStateMachineProvider` implementing `IStateMachineProvider`, which will generate instances of your `IStateMachine`. The code above tells Stateflow to use your `MyStateMachineProvider` instead of default.

### Some useful extensions

There are some extensions for Stateless which could be useful while configuring entity behavior:

| Method name | Description |
| --- | --- |
| `InternalTransition` | Adds an internal transition without any additional action to the state machine. |
| `InternalTransitionIf` | Adds an internal transition with guard condition and without any additional action to the state machine. |
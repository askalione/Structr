# StateMachine

The main "worker" of Structr.Stateflows package is an `IStateMachine` which provide main functionality related to behavior modelling. It specifies properties and methods allowing to determine whenever we could or couldn't perform certain actions depending on specified entity state. These are:

| Property name | Property type | Description |
| --- | --- | --- |
| `State` | TState | Represents current state of entity. `TState` type could variate but in most cases it's a some kind of enumeration. |
| `PermittedTriggers` | IEnumerable<TTrigger> | Set of permitted triggers (or actions) in current state of entity. `TTrigger` is also in most cases an enumeration. |

| Method name | Description |
| --- | --- |
| `CanFire` | Determines whenever specified trigger can be fired in current state. |
| `Fire` | Transition from the current state via the specified trigger. The target state is determined by the configuration of the current state. Actions associated with leaving the current state and entering the new one will be invoked. |

The default implementation of `IStateMachine` provided by Structr.Stateflows uses under the hood the popular lightweight state machine package for .NET - [Stateless](https://github.com/dotnet-state-machine/stateless) to perform it's methods. So all configuration process in `IStateMachineConfigurator` implementations (like `IssueStateMachineConfigurator`) should be done taking into account Stateless docs.

There are some extensions for Stateless which could be useful while configuring entity behavior:

| Method name | Description |
| --- | --- |
| `InternalTransition` | Adds an internal transition without any additional action to the state machine. |
| `InternalTransitionIf` | Adds an internal transition with guard condition and without any additional action to the state machine. |

When for some reason the Stateless in `IStateMachine` is not suitable for your needs, it could be replaced by any other implementation on registration step, using special options:

```csharp
serviceCollection.AddStateflows(x =>
    {
        x.ProviderType = typeof(CustomStateMachineProvider);
    },
    typeof(Program).Assembly);
```

This would work after you define your own implementation of `IStateMachine` (some `CustomStateMachine` for example) which would use some over mechanisms to power `CanFire` and other methods and properties of `IStateMachine`. After that just create `CustomStateMachineProvider` implementing `IStateMachineProvider`, which will generate instances of your `IStateMachine`. The code above tells Stateflow to use your `CustomStateMachineProvider` instead of default.
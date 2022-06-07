# Stateflows

**Structr.Stateflows** package provides functionality for modeling behavior of state-driven entities by creating and using state machines in your .NET application.
It provides simple yet functional API via `Stateflow` object that encapsulate methods and properties to monitor available actions and change state of controlled entity.

## Installation

Stateflows package is available on [NuGet](https://www.nuget.org/packages/Structr.Stateflows/). 

```
dotnet add package Structr.Stateflows
```

## Setup

Configure stateflow services: 

```csharp
services.AddStateflows(typeof(Program).Assembly);
```

`AddStateflows()` extension method performs registration of all configurator classes that implement `IStateMachineConfigurator` and stateflow providers that implement `IStateflowProvider`. 

| Param name | Param type | Description |
| --- | --- | --- |
| assembliesToScan | `params Assembly[]` | List of assemblies to search configurator classes and stateflow providers. |
| configureOptions | `Action<StateflowServiceOptions>` | Options to be used by state machine provider. | 

Additionally configure `IStateMachineProvider` service by specifying it's type and lifetime used `StateflowServiceOptions`.

`StateflowServiceOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| ProviderType | `Type` | Changes standard implementation of `IStateMachineProvider` to specified one. Default value is `typeof(StateMachineProvider)`. | 
| ProviderServiceLifetime | `ServiceLifetime` | Specifies the lifetime of an `IStateMachineProvider` service. Default value is `Scoped`. |

## Usage

The basic usage is:

Let's say we have some application where user could create `Issue` objects containing feedback or comments and publish them:

```csharp
public class Issue
{
    public int Id { get; set; }
    public IssueState State { get; set; }
    public string Comments { get; set; }
    public int AuthorId { get; set; }
    public int? AssigneeId { get; set; }
}
```

Every such `Issue` has one of these states:

```csharp
public enum IssueState
{
    Draft,
    Submitted,
    InProgress,
    Solved,
    Canceled
}
```

Each of such states imply different set of actions, some of which lead to change of state (like Submitting or Solving) and some not (like Editing)
Create an `enum` for all possible actions:

```csharp
public enum IssueAction
{
    Submit, // Change issue state: Draft ---> Submitted.    
    AcceptToWork, // Change issue state: Submitted ---> InProgress; Canceled ---> InProgress.
    Cancel, // Change issue state: InProgress ---> Canceled.
    Solve, // Change issue state: InProgress ---> Solved.
    Edit, // Doesn't change issue state.
    Delete, // Doesn't change issue state.
    ChangeAssignee // Doesn't change issue state.
}
```

Now u should create configurator class that implements `IStateMachineConfigurator` for configuring state machine:

```csharp
// StateMachineConfigurator is a abstract class allowing you to configure state machine synchronously.
// For asynchronously configuring inherit from IStateMachineConfigurator interface instead of StateMachineConfigurator class.
public class IssueStateMachineConfigurator : StateMachineConfigurator<Issue, IssueState, IssueAction>
{
    // As long as IssueStateMachineConfigurator is a normal .NET class with it's own constructor we could inject into it any dependencies needed.
    // Here we are adding some ICurrentUserService to be able to take info about current user while modeling Issue's behavior for its different states.
    private readonly ICurrentUserService _currentUserService;

    public IssueStateMachineConfigurator(ICurrentUserService currentUserService)
        => _currentUserService = currentUserService;

    // You should configure each issue state.
    protected override void Configure(Stateless.StateMachine<IssueState, IssueAction> stateMachine, Issue issue)
    {
        // Getting current user.
        ICurrentUser currentUser = _currentUserService.CurrentUser;
        // Getting info about current user for specified issue.
        bool isAdmin = currentUser.HasRole("Admin");
        bool isAuthor = issue.AuthorId == currentUser.Id;
        bool isAssignee = issue.AssigneeId == currentUser.Id;
        
        stateMachine.Configure(IssueState.Draft)
            .PermitIf(IssueAction.Submit, IssueState.Submitted, () => isAuthor)
            .InternalTransitionIf(IssueAction.Edit, () => isAuthor)
            .InternalTransitionIf(IssueAction.Delete, () => isAuthor);

        stateMachine.Configure(IssueState.Submitted)
            .PermitIf(IssueAction.AcceptToWork, IssueState.InProgress, () => isAdmin);

        stateMachine.Configure(IssueState.InProgress)
            .PermitIf(IssueAction.Cancel, IssueState.Canceled, () => isAssignee)
            .PermitIf(IssueAction.Solve, IssueState.Solved, () => isAssignee)
            .InternalTransitionIf(IssueAction.ChangeAssignee, () => isAssignee || isAdmin);
        
        stateMachine.Configure(IssueState.Canceled)
            .PermitIf(IssueAction.AcceptToWork, IssueState.InProgress, () => isAssignee)
            .InternalTransitionIf(IssueAction.ChangeAssignee, () => isAssignee || isAdmin);

        // IssueState.Solve don't be needed to configuring because this state doesn't have transitions.
    }
}
```

Then create `IssueStateflowProvider` that inheriting from `IStateflowProvider` and provides access to configured state machine and issue object:

```csharp
public interface IIssueStateflowProvider : IStateflowProvider<Issue, int, IssueState, IssueAction>
{}

public class IssueStateflowProvider : IIssueStateflowProvider
{
    private readonly IDbContext _dbContext;
    // This one is provided by Stateflow package and will give us an instance of the default state machine implementation.
    private readonly IStateMachineProvider _stateMachineProvider;

    public IssueStateflowProvider(IDbContext dbContext, IStateMachineProvider stateMachineProvider)
    {
        _dbContext = dbContext;
        _stateMachineProvider = stateMachineProvider;
    }

    public async Task<Stateflow<Issue, IssueState, IssueAction>> GetStateflowAsync(int id, CancellationToken cancellationToken = default)
    {
        // Getting issue from storage.
        Issue? issue = await _dbContext.Issues.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        // Getting state machine.
        IStateMachine<IssueState, IssueAction> stateMachine = await _stateMachineProvider.GetStateMachineAsync<Issue, IssueState, IssueAction>(entity: issue,
            stateAccessor: x => x.State,
            stateMutator: (x, state) => x.State = state,
            cancellationToken: cancellationToken);

        // Creating stateflow instance for given issue.
        var stateflow = new Stateflow<Issue, IssueState, IssueAction>(issue, stateMachine);

        return stateflow;
    }
}
```

The last step is to inject `IIssueStateflowProvider` service and use it:

```csharp
public class IssueService : IIssueService
{
    private readonly IDbContext _dbContext;
    private readonly IIssueStateflowProvider _stateflowProvider;
    private readonly ICurrentUserService _currentUserService;

    public IssueService(IDbContext dbContext, 
        IIssueStateflowProvider stateflowProvider,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _stateflowProvider = stateflowProvider;
        _currentUserService = currentUserService;
    }

    public async Task SubmitAsync(int issueId, CancellationToken cancellationToken = default)
    {
        Stateflow<Issue, IssueState, IssueAction> stateflow = await _stateflowProvider.GetStateflowAsync(issueId, cancellationToken);
        Issue issue = stateflow.Entity;
        IStateMachine<IssueState, IssueAction> stateMachine = stateflow.StateMachine;
        IssueAction action = IssueAction.Submit;

        if (stateMachine.CanFire(action) == false)
        {
            throw new StateflowException("Submit operation is not permitted.");
        }

        // Submitting issue while changing its state to "Submitted".
        stateMachine.Fire(action);

        // Save changes to database.
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AcceptToWorkAsync(int issueId, CancellationToken cancellationToken = default)
    {
        Stateflow<Issue, IssueState, IssueAction> stateflow = await _stateflowProvider.GetStateflowAsync(issueId, cancellationToken);
        Issue issue = stateflow.Entity;
        IStateMachine<IssueState, IssueAction> stateMachine = stateflow.StateMachine;
        IssueAction action = IssueAction.AcceptToWork;

        if (stateMachine.CanFire(action) == false)
        {
            throw new StateflowException("AcceptToWork operation is not permitted.");
        }

        // Submitting issue while changing its state to "AcceptToWork".
        stateMachine.Fire(action);

        // Set assignee to issue.
        ICurrentUser currentUser = _currentUserService.CurrentUser;
        issue.AssigneeId = currentUser.Id;

        // Save changes to database.
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /* Other actions implementations */
}
```

To check available actions for issue on the presentation layer (WebUI for example):

```csharp
// Base permitted actions interface.
public interface IPermittedActions<TAction>
{
    IEnumerable<TAction> PermittedActions { get; }
} 

// Extension for simplify checking available actions.
public static class PermittedActionsExtensions
{
    public static bool CanFire<TAction>(this IPermittedActions<TAction> context, TAction action)
        => context?.PermittedActions?.Contains(action) ?? false;
}

// Issue DTO.
public record IssueDto
{
    public int Id { get; init; }
    public IssueState State { get; init; }
    public string Comments { get; init; }
    public int AuthorId { get; init; }
    public int? AssigneeId { get; init; }
}

// Issue context DTO inherits from IPermittedActions
public record IssueContextDto, IPermittedActions<IssueAction>
{
    public IssueDto Issue { get; init; }
    public IEnumerable<IssueAction> PermittedActions { get; init; }
}

// AutoMapper Profile.
internal class IssueDtoProfile : Profile
{
    public IssueContextDtoProfile()
    {
        CreateMap<Issue, IssueDto>();
    }
}

// Query.
public record IssueContextGetByIdQuery : IOperation<IssueContextDto>
{
    public int Id { get; init; }
}

// Query handler.
internal class IssueContextGetByIdQueryHandler : AsyncOperationHandler<IssueContextGetByIdQuery, IssueContextDto>
{
    private readonly IDbContext _dbContext;
    private readonly IIssueStateflowProvider _stateflowProvider;
    private readonly IMapper _mapper;

    public IssueContextGetByIdQueryHandler(IDbContext dbContext,
        IIssueStateflowProvider stateflowProvider, 
        IMapper mapper)
    {
        _dbContext = dbContext;
        _stateflowProvider = stateflowProvider;
        _mapper = mapper;
    }

    public override async Task<IssueContextDto> HandleAsync(IssueContextGetByIdQuery query, CancellationToken cancellationToken)
    {
        Stateflow<Issue, IssueState, IssueAction> stateflow = await _stateflowProvider.GetStateflowAsync(issueId, cancellationToken);
        Issue issue = stateflow.Entity;
        IStateMachine<IssueState, IssueAction> stateMachine = stateflow.StateMachine;

        IssueContextDto result = new IssueContextDto 
        {
            Issue = _mapper.Map<IssueDto>(issue),
            PermittedActions = issue.PermittedTriggers;
        }

        return result;
    }
}
```

Then in any place of your presentation layer you can check issue action. For example in controller:

```csharp
public class IssuesController : Controller
{
    private readonly IOperationExecutor _executor;

    public IssuesController(IOperationExecutor executor)
        => _executor = executor;

    public async Task<IActionResult> Edit(int id)
    {
        IssueContextDto context = await _executor.ExecuteAsync(new IssueContextGetByIdQuery { Id = id });
        if (context.CanFire(IssueAction.Edit) == false)
        {
            throw new AccessDeniedException("Edit operation is not permitted.");
        }

        return View(context);
    }
}
```
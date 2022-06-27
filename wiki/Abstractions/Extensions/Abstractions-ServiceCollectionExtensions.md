# ServiceCollection extensions

## AddFactory

Registers service with different implementations determined by provided keys. This allows to instantiate different implementations of registered service depending on provided key already after the injection.

There are numbers of cases where we need to provide some method with service which direct implementation will be known only at a run-time. This usually occurs when some processor-type service is needed to work with objects which type will be known only after getting them from DB.
Great example here will processing of tickets in some HelpDesk application.

```csharp
public class Ticket
{
    public int Id { get; private set; }
    public TicketType Type { get; private set; }
    public string Description { get; private set; }
    /* some other data */
}
```

Tickets could have different types, so they will have different processing rules and life cycles.

```csharp
public enum TicketType
{
    ClientTicket,
    EmployeeTicket
}
```

Then any method working with tickets should get exemplar of some lifecycle manager service to do its type-specific job - `ITicketLifecycleManager`. Let it be some `ClientTicketLifecycleManager`. Here `AddFactory` method comes to our aid:

```csharp
services
    .AddFactory<TicketType, ITicketLifecycleManager>(new Dictionary<TicketType, Type> {
        { TicketType.ClientTicket, typeof(ClientTicketLifecycleManager) },
        { TicketType.EmployeeTicket, typeof(EmployeeTicketLifecycleManager) }
    })
```

This adds factory of lifecycle managers as a service to inject into target methods:

```csharp
public class TicketCloseCommandHandler : ICommand<TicketCloseCommand>
{
    private readonly Func<TicketType, ITicketLifecycleManager> _ticketLifecycleManagerFactory;
    private readonly DbContext _dbContext;

    public TicketCloseCommandHandler(Func<TicketType, ITicketLifecycleManager> ticketLifecycleManagerFactory,
    DbContext dbContext)
    {
        _ticketLifecycleManagerFactory = ticketLifecycleManagerFactory;
        _dbContext = dbContext;
    }

    public Task CloseTicket(int ticketId)
    {
        var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(x => x.Id == ticketId);
        /* do some stuff */
        var ticketLifecycleManager = _ticketLifecycleManagerFactory(ticket.Type);
        ticketLifecycleManager.ManageTicketClosing(ticket); // <--- this one is unique to ticket type and we got the one we needed
        /* do more stuff */
        await _dbContext.SaveChangesAsync();
    }
}

```

Done! We've managed closing ticket using its own lifecycle service implementation, despite we didn't know its type at start.
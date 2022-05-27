# Notices

**Structr.Notices** package is intended to help organize notifications in application.

## Installation

Notices package is available on [NuGet](https://www.nuget.org/packages/Structr.Notices/).

```
dotnet add package Structr.Notices
```

## Setup

Create a notice class that inherits from `INotice`, for example, `ConfirmationNotice`:

```csharp
public class ConfirmationNotice : INotice
{
    public int UserId { get; set; }
    public string Message { get; set; }
}
```

Create a notice handler class that inherits from `INoticeHandler` with generic parameter `ConfirmationNotice`, for example, with email notification:

```csharp
public class EmailConfirmationNoticeHandler :  : INoticeHandler<ConfirmationNotice>
{
    private readonly IDbContext _dbContext;
    private readonly IEmailSender _emailSender;

    public EmailFooEventHandler(IDbContext dbContext, IEmailSender emailSender)
    {
        /* Arguments null validation */
        _dbContext = dbContext;
        _emailSender = emailSender;
    }

    public Task HandleAsync(ConfirmationNotice notice, CancellationToken cancellationToken)
    {
        User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id = notice.UserId);
    
        _emailSender.Send(user.Email, notice.Message);
    }
}
```

And then setup notice services:

```csharp
services.AddNotices(typeof(ConfirmationNotice).Assembly);
```

## Usage

After setup you can use `INoticePublisher` in your application that allows you to send notifications asynchronously, for example:

```csharp
    public class ContractService
    {
        private readonly INoticePublisher _publisher;

        public ContractService(INoticePublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task ConfirmAsync(int id)
        {
            /* Confirm contract with Id == id */
            
            var confirmationNotice = new ConfirmationNotice
            { 
                UserId = contract.Customer.UserId,
                Message = $"Contract with number '{contract.Number}' confirmed."
            };
            
            await _publisher.PublishAsync(confirmationNotice);
        }
    }
```

## Extensions

You can implement your custom notice publisher `CustomNoticePublisher` that inherits from `INoticePublisher` and use it at setup, for example:

```csharp
services.AddNotices(options =>
    {
        options.PublisherType = typeof(CustomNoticePublisher);
    },
    typeof(ConfirmationNotice).Assembly);
```

You can override publisher service lifetime, for example:

```csharp
services.AddNotices(options =>
    {
        options.PublisherServiceLifetime = ServiceLifetime.Transient;
    },
    typeof(CustomNotice).Assembly)
```
# Notices

**Structr.Notices** package is intended to help organize notification dispatching in application.

## Installation

Notices package is available on [NuGet](https://www.nuget.org/packages/Structr.Notices/).

```
dotnet add package Structr.Notices
```

## Setup

Configure notice services:

```csharp
services.AddNotices(typeof(Program).Assembly);
```
`AddNotices()` extension method performs registration of notice publisher service `INoticePublisher` and notice handlers implementing `INoticeHandler` or inherited from `NoticeHandler` class.

| Param name | Param type | Description |
| --- | --- | --- |
| assembliesToScan | `params Assembly[]` | List of assemblies to search notice handlers. |
| configureOptions | `Action<NoticeServiceOptions>` | Options to be used by notices handling service. | 

Additionally configure `INoticePublisher` service by specifying it's type and lifetime used `NoticeServiceOptions`.

`NoticeServiceOptions` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| PublisherType | `Type` | Changes standard implementation of `INoticePublisher` to specified one. Default value is `typeof(NoticePublisher)`. | 
| PublisherServiceLifetime | `ServiceLifetime` | Specifies the lifetime of an `INoticePublisher` service. Default value is `Scoped`. |

## Usage

The main difference from [Structr.Operations](https://www.nuget.org/packages/Structr.Operations/) package is that notice can handling by any of handlers while operation can handling by only one handler.

The basic usage is:

Create a notice class that inherits from `INotice`:

```csharp
record UserSignedInNotice : INotice
{
    public int UserId { get; init; }
    public string IpAddress { get; init; }
}
```

Create a notice handlers class that inherits from `INoticeHandler` or `NoticeHandler`:

```csharp
// Email handler for example.
class EmailUserSignedInNoticeHandler : INoticeHandler<UserSignedInNotice>
{
    private readonly IDbContext _dbContext;
    private readonly IEmailSender _emailSender;

    public EmailUserSignedInNoticeHandler(IDbContext dbContext, IEmailSender emailSender)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
    }

    public Task HandleAsync(UserSignedInNotice notice, CancellationToken cancellationToken)
    {
        User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == notice.UserId, cancellationToken);
    
        await _emailSender.SendAsync(user.Email, $"Signed in successfully with IP-address: {notice.IpAddress}");
    }
}

// SMS handler example.
class SmsUserSignedInNoticeHandler : INoticeHandler<UserSignedInNotice>
{
    private readonly IDbContext _dbContext;
    private readonly ISmsSender _emailSender;

    public SmsUserSignedInNoticeHandler(IDbContext dbContext, ISmsSender smsSender)
    {
        _dbContext = dbContext;
        _smsSender = smsSender;
    }

    public Task HandleAsync(UserSignedInNotice notice, CancellationToken cancellationToken)
    {
        User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == notice.UserId, cancellationToken);
    
        await _smsSender.SendAsync(user.PhoneNumber, $"Signed in successfully with IP-address: {notice.IpAddress}");
    }
}
```

The last step is to inject `INoticePublisher` service and use it:

```csharp
class UserService
{
    private readonly INoticePublisher _publisher;

    public UserService(INoticePublisher publisher)
        => _publisher = publisher;

    public async Task SignIn(string email, string password, string ipAddress)
    {
        User user = await FindUserAsync(email, password);
        if (user != null)
        {
            /* Some sign in logic here */

            var notice = new UserSignedInNotice { UserId = user.Id, IpAddress = ipAddress };
            await _publisher.PublishAsync(notice);
        }
    }
}
```

Inherit from `NoticeHandler` class when you need synchronic-manner handler.
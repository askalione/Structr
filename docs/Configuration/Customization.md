# Customization

`OptionAttribute` allow you to customize the settings members. 

`OptionAttribute` properties:

| Property name | Property type | Description |
| --- | --- | --- |
| Alias | `string` | Alias for a settings member. |
| DefaultValue | `object` | Default value for a settings member. |
| EncryptionPassphrase | `string` | Passphrase thats used to encrypt the protected a settings member value. |

Let's for example you are using JSON file for OAuth settings:

```json
{
  "client_id": "6779ef20e75817b79602",
  "client_secret": "o9Bnp/NQO7pQsHJbYFciqVPGcrZf8Cmnamb3IQSDI7VHpZhyJbXPwA4KSDK9LQNh",
  "scopes": ""
}
```

Then you should have such settings class:

```csharp
public class OAuthSettings
{
    [Option(Alias = "client_id")]
    public string ClientId { get; set; }

    [Option(Alias = "client_secret", EncryptionPassphrase= "_bSk%nzi&")]
    public string ClientSecret { get; set; }

    [Option(DefaultValue = "read")]
    public string Scopes { get; set; }
}
```

With this setting up when you call settings in your application service:

```csharp
var settings = _configuration.Settings;
// This returns:
// {
//    ClientId = "6779ef20e75817b79602",
//    ClientSecret = "C07D3D9B-D411-4177-B4A4-699431C85176",
//    Scopes = "read"
// }
```
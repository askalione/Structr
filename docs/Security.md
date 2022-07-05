# Security

**Structr.Security** package contains classes and extension methods that will help in organization application security infrastructure, such as encryption services and handy Claims extensions. 

## Installation

Abstractions package is available on [NuGet](https://www.nuget.org/packages/Structr.Security/). 

```
dotnet add package Structr.Security
```

## Usage

* [Claims](#claims)
* [Hashes](#hashes)
* [Encryption](#encryption)

### Claims

Claims extension methods are mostly extensions for `ClaimsIdentity` and collection of `Claims` that could simplify working with adding, changing and removing claims.

Samples for some of `ClaimsIdentity` extensions:

```csharp
var claimsIdentity = new ClaimsIdentity();

// Add claims fluently without Claim constructors.
claimsIdentity.AddClaim("SomeClaimType", "1,25")
    .AddClaim("SomeClaimType", "3,57")
    .AddClaim("SomeAnotherClaimType", "value 1")
    .AddClaim("SomeAnotherClaimType", "value 2")
    .AddClaim("AndAnotherClaimType", "True");

// Replaces value for claim with type "SomeAnotherClaimType".
claimsIdentity.SetClaim("SomeAnotherClaimType", "1,25");

// Getting claim value converted to specified type. 
float someClaimValue = claimsIdentity.GetFirstClaim<float>("SomeClaimType"); // Returns 1.25F

// Removes claims with type "SomeClaimType".
claimsIdentity.RemoveAllClaims("SomeClaimType");

// Getting all claims values as a string with type "SomeAnotherClaimType".
IEnumerable<string> someAnotherClaimValues = claimsIdentity.FindAllValues("SomeAnotherClaimType");
```

Samples for some of `Claim` and collection of `Claim` extensions:

```csharp
var claims = new List<Claim>{ new Claim("Type1", "1,25"), new Claim("Type1", "3,57"), new Claim("Type2", "Value3") };

// Getting first claim with type "Type1".
Claim claim = claims.FindFirst("Type1"); // Returns `Claim("Type1", "1,25")`
// Get claim value
float claimValue = claim.GetValue<float>(); // Returns 1.25F

// Or getting first claim value with type "Type1" directly from collection.
claimValue = claims.GetFirstValue<float>("Type1");  // Returns 1.25F

// Or try getting claim value.
if (claims.TryGetFirstValue("Type1", out float value))
{
    claimValue = value;
}
```

### Hashes

`Md5Hasher` and `Pbkdf2Hasher` classes provides functionality for hashing input strings and verifying them.

Hashers methods:

| Method name | Return type | Description |
| --- | --- | --- |
| Hash | `string` | Hashes specified string using chosen hash algorithm. |
| Verify | `bool` | Verifies provided input string with specified hash, using chosen hash algorithm. |

Hash and verify password for example using PBKDF2 hash algorithm:

```csharp
// Sign up new user or change user password.
string email = "structr@structr.dev";
string password = "qwerty";
string passwordHash = Pbkdf2Hasher.Hash(password); // Create password hash.
User user = new User(email, passwordHash);
_dbContext.Users.Add(user);

// Sign in user with email and password.
string email = "structr@structr.dev";
string password = "qwerty";
User user = _dbContext.Users.FirstOrDefault(x => x.Email == email);
if (user != null) 
{
    bool isVerified = Pbkdf2Hasher.Verify(password, user.PasswordHash); // Verify input password with user password hash.
    if (isVerified)
    {
        /* Some sign in logic */
    }
}
```

### Encryption

`StringEncryptor` class provides functionality for encrypting and decrypting strings using passphrase.

`StringEncryptor` methods:

| Method name | Return type | Description |
| --- | --- | --- |
| Encrypt | `string` | Encrypts input string using specified passphrase. |
| Decrypt | `string` | Decrypts input encrypted string using specified passphrase. |

Encrypt and decrypt secure settings for example:

```csharp
string clientSecret = "4E46E591-987F-443A-96BA-38804D7E1617";
string passphrase = "Structr";

// Encrypt client secret with passphrase
string encryptedClientSecret = StringEncryptor.Encrypt(clientSecret, passphrase);
File.WriteAllText("secrets.config", encryptedClientSecret);

encryptedClientSecret = File.ReadlAllText("secrets.config"); 
// Decrypt client secret with passphrase
clientSecret = StringEncryptor.Decrypt(encryptedClientSecret, passphrase);
```

`EncryptingJsonConverter` class will be useful when using [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) package for JSON serialization:

Define typed-class for settings:

```csharp
using Newtonsoft.Json;
using Structr.Security.Json;

public class ClientSettings
{
    public string ClientId { get; set; }

    [JsonConverter(typeof(EncryptingJsonConverter))]
    public string ClientSecret { get; set; }
}
```

Then serialize settings to JSON:

```csharp
using Newtonsoft.Json;

var user = new User 
{ 
    ClientId = "6779ef20e75817b79602",
    ClientSecret = "4E46E591-987F-443A-96BA-38804D7E1617"
};
string encryptedJson = JsonConvert.SerializeObject(user, Formatting.Indented);
File.WriteAllText("secrets.json", encryptedJson);
```
# Security

**Structr.Security** package contains classes and extension methods that will help in organization application security infrastructure, such as encryption services and handy Claims extensions. 

## Installation

Abstractions package is available on [NuGet](https://www.nuget.org/packages/Structr.Security/). 

```
dotnet add package Structr.Security
```

## Usage

TODO

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
if (claims.TryGetFirstValue("Type1", float value))
{
    claimValue = value;
}
```

----------------------------------------------------
----------------------------------------------------
----------------------------------------------------

## Claims extensions

These methods are mostly extensions for `ClaimsIdentity` and `ClaimsPrincipal` and could simplify working with adding, changing and removing claims from this objects.

```csharp
var claimsIdentity = new ClaimsIdentity();

// Add claims fluently without Claim constructors.
claimsIdentity.AddClaim("SomeClaimType", "value 1")
    .AddClaim("SomeClaimType", "value 2")
    .AddClaim("SomeAnotherClaimType", "3")
    .AddClaim("AndAnotherClaimType", "4");

// Replaces value for claim with type "SomeAnotherClaimType"
claimsIdentity.SetClaim("SomeAnotherClaimType", "new Value");

// Removes claims with type "SomeClaimType"
claimsIdentity.RemoveClaims("SomeClaimType");

/* And some extensions examples for ClaimsPrincipal: */
var claimsPrincipal = new ClaimsPrincipal();

// Adds claims with type "SomeClaimType" to principal.
claimsPrincipal.SetClaims("SomeClaimType", new string[] { "1.25", "2,75" });

// Getting claim value converted to specified type. 
var result = claimsPrincipal.GetClaim<float>("SomeClaimType"); // Returns 1.25F
```

## Hashers

`Md5Hasher` and `Pbkdf2Hasher` provide functionality for hashing input strings and verifying them.

| Method name | Description |
| --- | --- |
| Hash | Hashes specified string using MD5 or PBKDF2 hash algorithm.
| Verify | Verifies provided input string with specified hash, using MD5 or PBKDF2 hash algorithm.

## StringEncryptor

`StringEncryptor` class provides functionality for encrypting and decrypting strings using passphrase.

| Method name | Description |
| --- | --- |
| Encrypt | Encrypts input string using specified passphrase.
| Decrypt | Decrypts input encrypted string using specified passphrase.
# Security

**Structr.Security** package contains classes and extension methods that will help in organization application security infrastructure, such as encryption services and handy Claims extensions. 

## Installation

Abstractions package is available on [NuGet](https://www.nuget.org/packages/Structr.Security/). 

```
dotnet add package Structr.Security
```

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
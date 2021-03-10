# Argon2 Password Hasher for ASP.NET Core Identity

[![NuGet](https://img.shields.io/nuget/v/ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher.svg)](https://www.nuget.org/packages/ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher/)

An implementation of IPasswordHasher<TUser> using [libsodium-core](https://github.com/tabrath/libsodium-core).

## Installation

```csharp
services.AddIdentity<TUser, TRole>();
services.AddScoped<IPasswordHasher<TUser>, Argon2PasswordHasher<TUser>>();
```

### Options

- **Strength**: Argon2HashStrength

Register with:

```csharp
services.Configure<Argon2PasswordHasherOptions>(options => {
    options.Strength = Argon2HashStrength.Interactive;
});
```

## .NET Support

This library supports Current and LTS versions of .NET.

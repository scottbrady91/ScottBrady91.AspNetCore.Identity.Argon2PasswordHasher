# Argon2 Password Hasher for ASP.NET Core Identity (ASP.NET Identity 3)

[![NuGet](https://img.shields.io/nuget/v/ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher.svg)](https://www.nuget.org/packages/ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher/)
[![Build Status](https://travis-ci.org/Atulin/Argon2PasswordHasher.svg?branch=master)](https://travis-ci.org/Atulin/Argon2PasswordHasher)

An implementation of IPasswordHasher<TUser> using [libsodium-core](https://github.com/tabrath/libsodium-core).

## Installation

```cs
services.AddIdentity<TUser, TRole>();
services.AddScoped<IPasswordHasher<TUser>, Argon2PasswordHasher<TUser>>();
```

### Options

 - **Strength**: Argon2HashStrength

Register with:
```cs
services.Configure<Argon2PasswordHasherOptions>(options => {
	options.Strength = Argon2HashStrength.Interactive;
});
```

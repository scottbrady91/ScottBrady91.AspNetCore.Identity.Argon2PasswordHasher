# Argon2 Password Hasher for ASP.NET Core Identity (ASP.NET Identity 3)

![](https://github.com/Atulin/Argon2PasswordHasher/workflows/netcore-2.1/badge.svg)
![](https://github.com/Atulin/Argon2PasswordHasher/workflows/netcore-2.2/badge.svg)
![](https://github.com/Atulin/Argon2PasswordHasher/workflows/netcore-3.0/badge.svg)
![](https://github.com/Atulin/Argon2PasswordHasher/workflows/netcore-3.1/badge.svg)

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

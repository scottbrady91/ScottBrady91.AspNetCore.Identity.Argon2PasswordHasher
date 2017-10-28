# Argon2 Password Hasher for ASP.NET Core Identity (ASP.NET Identity 3)

An implementation of IPasswordHasher<TUser> using [libsodium-core](https://github.com/tabrath/libsodium-core).

## Installation

```
services.AddScoped<IPasswordHasher<ApplicationUser>, Argon2PasswordHasher<ApplicationUser>>();
```

### Options

 - **Strength**: Argon2HashStrength

Register with:
```
services.Configure<Argon2PasswordHasherOptions>(options => {
	options.Strength = Argon2HashStrength.Interactive;
});
```

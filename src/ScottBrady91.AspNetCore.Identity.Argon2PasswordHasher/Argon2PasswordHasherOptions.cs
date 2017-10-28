using Sodium;

namespace ScottBrady91.AspNetCore.Identity
{
    public class Argon2PasswordHasherOptions
    {
        public Argon2HashStrength Strength { get; set; } = Argon2HashStrength.Interactive;
    }
}
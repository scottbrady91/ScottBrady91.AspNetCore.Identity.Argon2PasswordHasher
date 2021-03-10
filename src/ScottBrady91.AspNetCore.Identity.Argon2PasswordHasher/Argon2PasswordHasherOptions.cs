namespace ScottBrady91.AspNetCore.Identity
{
    /// <summary>
    /// Options for Argon2PasswordHasher.
    /// </summary>
    public class Argon2PasswordHasherOptions
    {
        /// <summary>
        /// Hash strength using pre-defined strengths from libsodium
        /// </summary>
        public Argon2HashStrength Strength { get; set; } = Argon2HashStrength.Interactive;
    }
}
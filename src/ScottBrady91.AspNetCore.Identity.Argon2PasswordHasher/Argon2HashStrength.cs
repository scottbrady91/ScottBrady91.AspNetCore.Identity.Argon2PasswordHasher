namespace ScottBrady91.AspNetCore.Identity
{
    /// <summary>
    /// Hash strengths for Argon password hasher
    /// </summary>
    public enum Argon2HashStrength
    {
        /// <summary>
        /// For interactive sessions (fast: uses 32MB of RAM)
        /// </summary>
        Interactive = 0,
        
        /// <summary>
        /// For normal use (moderate: uses 128MB of RAM)
        /// </summary>
        Moderate,
        
        /// <summary>
        /// For highly sensitive data (slow: uses 512MB of RAM)
        /// </summary>
        Sensitive,
        
        /// <summary>
        /// For medium use (medium: uses 64MB of RAM)
        /// </summary>
        Medium
    }
}
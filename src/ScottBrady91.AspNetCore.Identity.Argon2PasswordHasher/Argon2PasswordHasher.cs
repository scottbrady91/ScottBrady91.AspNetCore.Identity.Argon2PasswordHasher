using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sodium;

namespace ScottBrady91.AspNetCore.Identity
{
    /// <summary>
    /// ASP.NET Core Identity password hasher using the Argon2id password hashing algorithm.
    /// </summary>
    /// <typeparam name="TUser">your ASP.NET Core Identity user type (e.g. IdentityUser). User is not used by this implementation</typeparam>
    public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private readonly Argon2PasswordHasherOptions options;

        /// <summary>
        /// Creates a new Argon2PasswordHasher.
        /// </summary>
        /// <param name="optionsAccessor">optional Argon2PasswordHasherOptions</param>
        public Argon2PasswordHasher(IOptions<Argon2PasswordHasherOptions> optionsAccessor = null)
        {
            options = optionsAccessor?.Value ?? new Argon2PasswordHasherOptions();
        }

        /// <summary>
        /// Hashes a password using Argon2id.
        /// </summary>
        /// <param name="user">not used for this implementation</param>
        /// <param name="password">plaintext password</param>
        /// <returns>hashed password</returns>
        /// <exception cref="ArgumentNullException">missing plaintext password</exception>
        public virtual string HashPassword(TUser user, string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            
            return PasswordHash.ArgonHashString(password, ParseStrength());
        }

        /// <summary>
        /// Verifies a plaintext password against a stored hash.
        /// </summary>
        /// <param name="user">not used for this implementation</param>
        /// <param name="hashedPassword">the stored, hashed password</param>
        /// <param name="providedPassword">the plaintext password to verify against the stored hash</param>
        /// <returns>If the password matches the stored password. Returns SuccessRehashNeeded if the work factor has changed</returns>
        /// <exception cref="ArgumentNullException">missing plaintext password or hashed password</exception>
        public virtual PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword)) throw new ArgumentNullException(nameof(hashedPassword));
            if (string.IsNullOrWhiteSpace(providedPassword)) throw new ArgumentNullException(nameof(providedPassword));

            var isValid = PasswordHash.ArgonHashStringVerify(hashedPassword, providedPassword);

            if (isValid && PasswordHash.ArgonPasswordNeedsRehash(hashedPassword, ParseStrength()))
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }
            
            return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }

        private PasswordHash.StrengthArgon ParseStrength()
        {
            switch (options.Strength)
            {
                case Argon2HashStrength.Interactive:
                    return PasswordHash.StrengthArgon.Interactive;
                case Argon2HashStrength.Moderate:
                    return PasswordHash.StrengthArgon.Moderate;
                case Argon2HashStrength.Sensitive:
                    return PasswordHash.StrengthArgon.Sensitive;
                case Argon2HashStrength.Medium:
                    return PasswordHash.StrengthArgon.Medium;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
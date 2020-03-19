using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sodium;

namespace Argon2PasswordHasher
{
    public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private readonly Argon2PasswordHasherOptions _options;

        public Argon2PasswordHasher(IOptions<Argon2PasswordHasherOptions> optionsAccessor = null)
        {
            _options = optionsAccessor?.Value ?? new Argon2PasswordHasherOptions();
        }

        public virtual string HashPassword(TUser user, string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            
            return PasswordHash.ArgonHashString(password, ParseStrength());
        }

        public virtual PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (providedPassword == null) throw new ArgumentNullException(nameof(providedPassword));

            var isValid = PasswordHash.ArgonHashStringVerify(hashedPassword, providedPassword);

            return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }

        private PasswordHash.StrengthArgon ParseStrength()
        {
            switch (_options.Strength)
            {
                case Argon2HashStrength.Interactive:
                    return PasswordHash.StrengthArgon.Interactive;
                case Argon2HashStrength.Moderate:
                    return PasswordHash.StrengthArgon.Moderate;
                case Argon2HashStrength.Sensitive:
                    return PasswordHash.StrengthArgon.Sensitive;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
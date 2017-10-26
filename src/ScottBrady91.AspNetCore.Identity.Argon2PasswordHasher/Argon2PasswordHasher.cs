using System;
using Microsoft.AspNetCore.Identity;
using Sodium;

namespace ScottBrady91.AspNetCore.Identity
{
    public class Argon2PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        public string HashPassword(TUser user, string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            return PasswordHash.ArgonHashString(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (providedPassword == null) throw new ArgumentNullException(nameof(providedPassword));

            var isValid = PasswordHash.ArgonHashStringVerify(hashedPassword, providedPassword);

            return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
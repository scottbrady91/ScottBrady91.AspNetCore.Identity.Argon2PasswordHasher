using System;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Sodium;
using Xunit;

namespace ScottBrady91.AspNetCore.Identity.BCryptPasswordHasher.Tests
{
    public class BCryptPasswordHasherTests
    {
        [Fact]
        public void HashPassword_WithDefaultSettings_ExpectVerifiableHash()
        {
            var password = Guid.NewGuid().ToString();

            var hasher = new Argon2PasswordHasher<string>();
            var hashedPassword = hasher.HashPassword("", password);

            PasswordHash.ArgonHashStringVerify(hashedPassword, password).Should().BeTrue();
        }
        
        [Fact]
        public void VerifyHashedPassword_WithDefaultSettings_ExpectSuccess()
        {
            var password = Guid.NewGuid().ToString();
            var hashedPassword = PasswordHash.ArgonHashString(password);

            var hasher = new Argon2PasswordHasher<string>();

            hasher.VerifyHashedPassword("", hashedPassword, password).Should().Be(PasswordVerificationResult.Success);
        }
        
        [Fact]
        public void VerifyHashedPassword_WhenSuppliedPasswordDoesNotMatch_ExpectFailure()
        {
            var password = Guid.NewGuid().ToString();
            var hashedPassword = PasswordHash.ArgonHashString(Guid.NewGuid().ToString());

            var hasher = new Argon2PasswordHasher<string>();

            hasher.VerifyHashedPassword("", hashedPassword, password).Should().Be(PasswordVerificationResult.Failed);
        }
    }
}
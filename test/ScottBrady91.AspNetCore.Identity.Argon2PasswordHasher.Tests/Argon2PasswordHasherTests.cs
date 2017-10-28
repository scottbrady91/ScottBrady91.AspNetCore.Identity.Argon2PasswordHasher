using System;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Sodium;
using Xunit;
using Microsoft.Extensions.Options;

namespace ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher.Tests
{
    public class Argon2PasswordHasherTests
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
        public void HashPassword_WithCustomStrength_ExpectVerifiableHash()
        {
            var password = Guid.NewGuid().ToString();

            var hasher = new Argon2PasswordHasher<string>(
                new OptionsWrapper<Argon2PasswordHasherOptions>(
                    new Argon2PasswordHasherOptions {Strength = Argon2HashStrength.Sensitive}));
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
        public void VerifyHashedPassword_WithCustomStrength_ExpectSuccess()
        {
            var password = Guid.NewGuid().ToString();
            var hashedPassword = PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Sensitive);

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
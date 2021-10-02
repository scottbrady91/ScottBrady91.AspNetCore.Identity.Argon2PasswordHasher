using System;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Sodium;
using Xunit;
using Microsoft.Extensions.Options;

namespace ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher.Tests
{
    public class Argon2PasswordHasherTests
    {
        private Argon2PasswordHasherOptions options = new Argon2PasswordHasherOptions();
        
        private Argon2PasswordHasher<string> CreateSut() =>
            new Argon2PasswordHasher<string>(
                options != null ? new OptionsWrapper<Argon2PasswordHasherOptions>(options) : null);

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void HashPassword_WhenPasswordIsNullOrWhitespace_ExpectArgumentNullException(string password)
        {
            var sut = CreateSut();
            Assert.Throws<ArgumentNullException>(() => sut.HashPassword(null, password));
        }
        
        [Fact]
        public void HashPassword_WithDefaultSettings_ExpectVerifiableHash()
        {
            var password = Guid.NewGuid().ToString();

            var sut = CreateSut();
            var hashedPassword = sut.HashPassword("", password);

            PasswordHash.ArgonHashStringVerify(hashedPassword, password).Should().BeTrue();
        }

        [Fact]
        public void HashPassword_WhenCalledMultipleTimesWithSamePlaintext_ExpectDifferentHash()
        {
            var password = Guid.NewGuid().ToString();

            var sut = CreateSut();
            var hashedPassword1 = sut.HashPassword("", password);
            var hashedPassword2 = sut.HashPassword("", password);

            hashedPassword1.Should().NotBe(hashedPassword2);
        }

        [Fact]
        public void HashPassword_WithCustomStrength_ExpectVerifiableHash()
        {
            var password = Guid.NewGuid().ToString();

            options.Strength = Argon2HashStrength.Sensitive;
            var sut = CreateSut();
            
            var hashedPassword = sut.HashPassword("", password);

            PasswordHash.ArgonHashStringVerify(hashedPassword, password).Should().BeTrue();
        }

        [Fact]
        public void HashPassword_ExpectNoNullTerminatedStrings()
        {
            var sut = CreateSut();

            var hashedPassword = sut.HashPassword("", Guid.NewGuid().ToString());

            hashedPassword.Last().Should().NotBe('\0');
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyHashedPassword_WhenHashedPasswordIsNullOrWhitespace_ExpectArgumentNullException(string hashedPassword)
        {
            var sut = CreateSut();
            Assert.Throws<ArgumentNullException>(() => sut.VerifyHashedPassword(null, hashedPassword, Guid.NewGuid().ToString()));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void VerifyHashedPassword_WhenPasswordIsNullOrWhitespace_ExpectArgumentNullException(string password)
        {
            var sut = CreateSut();
            Assert.Throws<ArgumentNullException>(() => sut.VerifyHashedPassword(null, Guid.NewGuid().ToString(), password));
        }

        [Fact]
        public void VerifyHashedPassword_WithDefaultSettings_ExpectSuccess()
        {
            var password = Guid.NewGuid().ToString();
            var hashedPassword = PasswordHash.ArgonHashString(password);

            var sut = CreateSut();

            sut.VerifyHashedPassword("", hashedPassword, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void VerifyHashedPassword_WithCustomStrength_ExpectSuccess()
        {
            var password = Guid.NewGuid().ToString();
            var hashedPassword = PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Sensitive);

            options.Strength = Argon2HashStrength.Sensitive;
            var sut = CreateSut();

            sut.VerifyHashedPassword("", hashedPassword, password).Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void VerifyHashedPassword_WhenSuppliedPasswordDoesNotMatch_ExpectFailure()
        {
            var password = Guid.NewGuid().ToString();
            var hashedPassword = PasswordHash.ArgonHashString(Guid.NewGuid().ToString());

            var sut = CreateSut();

            sut.VerifyHashedPassword("", hashedPassword, password).Should().Be(PasswordVerificationResult.Failed);
        }

        [Fact]
        public void VerifyHashedPassword_WhenPasswordHashedWithLowerStrength_ExpectSuccessRehashNeeded()
        {
            var password = Guid.NewGuid().ToString();
            var hashedPassword = PasswordHash.ArgonHashString(password, PasswordHash.StrengthArgon.Medium);

            options.Strength = Argon2HashStrength.Sensitive;
            var sut = CreateSut();

            sut.VerifyHashedPassword("", hashedPassword, password).Should().Be(PasswordVerificationResult.SuccessRehashNeeded);
        }
    }
}
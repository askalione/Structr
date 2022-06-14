using FluentAssertions;
using Structr.Security;
using System;
using Xunit;

namespace Structr.Tests.Security
{
    public class StringEncryptorTests
    {
        [Fact]
        public void Could_be_decrypted()
        {
            // Arrange
            var str = "Encryption test_phrase 123 !@#";

            // Act
            var encrypted = StringEncryptor.Encrypt(str, "myPassphrase");
            var decrypted = StringEncryptor.Decrypt(encrypted, "myPassphrase");

            // Assert
            encrypted.Should().HaveLength(64).And.NotBe(str);
            decrypted.Should().Be(str);
        }

        [Fact]
        public void Throws_when_input_is_null_or_empty()
        {
            // Act
            Action act = () => StringEncryptor.Encrypt("", "myPassphrase");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*input*");
        }

        [Fact]
        public void Throws_when_passphrase_is_null_or_empty()
        {
            // Act
            Action act = () => StringEncryptor.Encrypt("abc", "");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*passphrase*");
        }


    }
}
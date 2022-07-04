using FluentAssertions;
using Structr.AspNetCore.Json;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Json
{
    public class JsonResponseTests
    {
        [Fact]
        public void Ctor_ok()
        {
            // Act
            var result = new JsonResponse(true);

            // Assert
            result.Ok.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Data.Should().BeNull();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_ok_message_when_ok_true()
        {
            // Act
            var result = new JsonResponse(true, "Message1");

            // Assert
            result.Ok.Should().BeTrue();
            result.Message.Should().Be("Message1");
            result.Data.Should().BeNull();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_ok_message_when_ok_false()
        {
            // Act
            var result = new JsonResponse(false, "Message1");

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message1");
            result.Data.Should().BeNull();
            result.Errors.Should().BeEquivalentTo(new[] { new JsonResponseError("Message1") });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void Ctor_ok_message_throws_when_message_is_null_or_white_space(string message)
        {
            // Act
            Action act = () => new JsonResponse(true, message);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_ok_data()
        {
            // Act
            var result = new JsonResponse(true, new DateTime(2018, 01, 18));

            // Assert
            result.Ok.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Data.Should().Be(new DateTime(2018, 01, 18));
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_ok_message_data()
        {
            // Act
            var result = new JsonResponse(true, "Message1", new DateTime(2018, 01, 18));

            // Assert
            result.Ok.Should().BeTrue();
            result.Message.Should().Be("Message1");
            result.Data.Should().Be(new DateTime(2018, 01, 18));
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_errors()
        {
            // Arrange
            var errors = new[] { new JsonResponseError("Message2"), new JsonResponseError("Message3") };

            // Act
            var result = new JsonResponse(errors);

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message2");
            result.Data.Should().BeNull();
            result.Errors.Should().Equal(errors);
        }

        [Fact]
        public void Ctor_errors_when_empty()
        {
            // Act
            var result = new JsonResponse(new JsonResponseError[] { });

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().BeNull();
            result.Data.Should().BeNull();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_errors_throws_when_errors_is_null()
        {
            // Act
            Action act = () => new JsonResponse(errors: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_errors_data()
        {
            // Arrange
            var errors = new[] { new JsonResponseError("Message2"), new JsonResponseError("Message3") };

            // Act
            var result = new JsonResponse(errors, new DateTime(2018, 01, 18));

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message2");
            result.Data.Should().Be(new DateTime(2018, 01, 18));
            result.Errors.Should().Equal(errors);
        }

        [Fact]
        public void Ctor_errorMessages()
        {
            // Arrange
            var errorMessages = new[] { "Message2", "Message3" };

            // Act
            var result = new JsonResponse(errorMessages);

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message2");
            result.Data.Should().BeNull();
            result.Errors.Should().SatisfyRespectively(
                x =>
                {
                    x.Message.Should().Be("Message2");
                },
                x =>
                {
                    x.Message.Should().Be("Message3");
                }
            );
        }

        [Fact]
        public void Ctor_errorMessages_throws_when_errorMessages_is_null()
        {
            // Act
            Action act = () => new JsonResponse(errorMessages: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_errorMessages_when_empty()
        {
            // Act
            var result = new JsonResponse(new string[] { });

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().BeNull();
            result.Data.Should().BeNull();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_errorMessages_data()
        {
            // Arrange
            var errorMessages = new[] { "Message2", "Message3" };

            // Act
            var result = new JsonResponse(errorMessages, new DateTime(2018, 01, 18));

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message2");
            result.Data.Should().Be(new DateTime(2018, 01, 18));
            result.Errors.Should().SatisfyRespectively(
                x =>
                {
                    x.Message.Should().Be("Message2");
                },
                x =>
                {
                    x.Message.Should().Be("Message3");
                }
            );
        }
    }
}

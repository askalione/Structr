using FluentAssertions;
using Structr.AspNetCore.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.AspNetCore.Json
{
    public class JsonResultTests
    {
        [Fact]
        public void Ctor_ok()
        {
            // Act
            var result = new JsonResult(true);

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
            var result = new JsonResult(true, "Message1");

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
            var result = new JsonResult(false, "Message1");

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message1");
            result.Data.Should().BeNull();
            result.Errors.Should().BeEquivalentTo(new[] { new JsonError("Message1") });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void Ctor_ok_message_throws_when_message_is_null_or_white_space(string message)
        {
            // Act
            Action act = () => new JsonResult(true, message);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_ok_data()
        {
            // Act
            var result = new JsonResult(true, new DateTime(2018, 01, 18));

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
            var result = new JsonResult(true, "Message1", new DateTime(2018, 01, 18));

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
            var errors = new[] { new JsonError("Message2"), new JsonError("Message3") };

            // Act
            var result = new JsonResult(errors);

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
            var result = new JsonResult(new JsonError[] { });

            // Assert
            result.Ok.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Data.Should().BeNull();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_errors_throws_when_errors_is_null()
        {
            // Act
            Action act = () => new JsonResult(errors: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_errors_data()
        {
            // Arrange
            var errors = new[] { new JsonError("Message2"), new JsonError("Message3") };

            // Act
            var result = new JsonResult(errors, new DateTime(2018, 01, 18));

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message2");
            result.Data.Should().Be(new DateTime(2018, 01, 18));
            result.Errors.Should().Equal(errors);
        }

        [Fact]
        public void Ctor_errorsMessages()
        {
            // Arrange
            var errorsMessages = new[] { "Message2", "Message3" };

            // Act
            var result = new JsonResult(errorsMessages);

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message2");
            result.Data.Should().BeNull();
            result.Errors.Should().SatisfyRespectively(
                x => {
                    x.Message.Should().Be("Message2");
                },
                x => {
                    x.Message.Should().Be("Message3");
                }
            );
        }

        [Fact]
        public void Ctor_errorsMessages_throws_when_errorsMessages_is_null()
        {
            // Act
            Action act = () => new JsonResult(errorsMessages: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_errorsMessages_when_empty()
        {
            // Act
            var result = new JsonResult(new string[] { });

            // Assert
            result.Ok.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Data.Should().BeNull();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_errorsMessages_data()
        {
            // Arrange
            var errorsMessages = new[] { "Message2", "Message3" };

            // Act
            var result = new JsonResult(errorsMessages, new DateTime(2018, 01, 18));

            // Assert
            result.Ok.Should().BeFalse();
            result.Message.Should().Be("Message2");
            result.Data.Should().Be(new DateTime(2018, 01, 18));
            result.Errors.Should().SatisfyRespectively(
                x => {
                    x.Message.Should().Be("Message2");
                },
                x => {
                    x.Message.Should().Be("Message3");
                }
            );
        }
    }
}

using FluentAssertions;
using Structr.AspNetCore.Json;
using Structr.Tests.AspNetCore.TestUtils;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.AspNetCore.Json
{
    public class JsonControllerExtensionsTests
    {
        [Fact]
        public void JsonResponse_with_ok()
        {
            // Act
            var result = new TestController().JsonResponse(true);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(true));
        }

        [Fact]
        public void JsonResponse_with_ok_and_message()
        {
            // Act
            var result = new TestController().JsonResponse(true, "Message1");

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(true, "Message1"));
        }

        [Fact]
        public void JsonResponse_with_ok_and_message_and_data()
        {
            // Act
            var result = new TestController().JsonResponse(true, "Message1", new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(true, "Message1", new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonResponse_with_ok_and_data()
        {
            // Act
            var result = new TestController().JsonResponse(true, new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(true, new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonSuccess()
        {
            // Act
            var result = new TestController().JsonSuccess();

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(true));
        }

        [Fact]
        public void JsonSuccess_with_message()
        {
            // Act
            var result = new TestController().JsonSuccess("Message1");

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(true, "Message1"));
        }

        [Fact]
        public void JsonSuccess_with_message_and_data()
        {
            // Act
            var result = new TestController().JsonSuccess("Message1", new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(true, "Message1", new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonError()
        {
            // Act
            var result = new TestController().JsonError();

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(false));
        }

        [Fact]
        public void JsonError_with_message()
        {
            // Act
            var result = new TestController().JsonError("Message1");

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(false, "Message1"));
        }

        [Fact]
        public void JsonError_with_message_and_data()
        {
            // Act
            var result = new TestController().JsonError("Message1", new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(false, "Message1", new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonError_with_messages()
        {
            // Act
            var result = new TestController().JsonError(new[] { "Message2", "Message3" });

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(new[] { "Message2", "Message3" }));
        }

        [Fact]
        public void JsonError_with_messages_and_data()
        {
            // Act
            var result = new TestController().JsonError(new[] { "Message2", "Message3" }, new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(new[] { "Message2", "Message3" }, new DateTime(2018, 01, 18)));
        }

        [Fact]
        public void JsonError_with_errors()
        {
            // Arrange
            List<JsonResponseError> errors = new List<JsonResponseError> { new JsonResponseError("Message2"), new JsonResponseError("Message3") };

            // Act
            var result = new TestController().JsonError(errors);

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(errors));
        }

        [Fact]
        public void JsonError_with_errors_and_data()
        {
            // Arrange
            List<JsonResponseError> errors = new List<JsonResponseError> { new JsonResponseError("Message2"), new JsonResponseError("Message3") };

            // Act
            var result = new TestController().JsonError(errors, new DateTime(2018, 01, 18));

            // Assert
            result.Value.Should()
                .BeEquivalentTo(new JsonResponse(errors, new DateTime(2018, 01, 18)));
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace Structr.Abstractions.Exceptions
{
    public class ForbiddenOperationException : AdHocException
    {
        public ForbiddenOperationException() : base() { }
        public ForbiddenOperationException(string message) : base(message) { }
        public ForbiddenOperationException(string message, Exception innerException) : base(message, innerException) { }
        protected ForbiddenOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

using System;
using System.Runtime.Serialization;

namespace Structr.Abstractions.Exceptions
{
    public class AccessDeniedException : AdHocException
    {
        public AccessDeniedException() : base() { }
        public AccessDeniedException(string message) : base(message) { }
        public AccessDeniedException(string message, Exception innerException) : base(message, innerException) { }
        protected AccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

using System;
using System.Runtime.Serialization;

namespace Structr.Abstractions.Exceptions
{
    public abstract class AdHocException : Exception
    {
        public AdHocException() : base() { }
        public AdHocException(string message) : base(message) { }
        public AdHocException(string message, Exception innerException) : base(message, innerException) { }
        protected AdHocException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

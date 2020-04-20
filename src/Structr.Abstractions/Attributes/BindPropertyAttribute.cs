using System;

namespace Structr.Abstractions.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class BindPropertyAttribute : Attribute
    {
        public string PropertyName { get; }
        public object PropertyValue { get; }

        public BindPropertyAttribute(string propertyName, object propertyValue)
        {
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}

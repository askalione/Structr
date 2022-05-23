using System;

namespace Structr.Abstractions.Attributes
{
    /// <summary>
    /// Provides possibility of attaching custom data to enumeration elements.
    /// using <see cref="Helpers.BindHelper.Bind{T, TEnum}(Action{T, TEnum})"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public sealed class BindPropertyAttribute : Attribute
    {
        /// <summary>
        /// Name of property to search while populating new-created instance.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Value to insert.
        /// </summary>
        public object PropertyValue { get; }

        /// <summary>
        /// Provides possibility of attaching custom data to enumeration elements with further using it in binding proccess.
        /// </summary>
        /// <param name="propertyName">Name of property to search while populating new-created instance.</param>
        /// <param name="propertyValue">Value to insert.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BindPropertyAttribute(string propertyName, object propertyValue)
        {
            Ensure.NotEmpty(propertyName, nameof(propertyName));

            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}

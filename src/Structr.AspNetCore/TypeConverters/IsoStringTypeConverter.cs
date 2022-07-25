using System;
using System.ComponentModel;
using System.Globalization;

namespace Structr.AspNetCore.TypeConverters
{
    /// <summary>
    /// Provides functionality for converting an instance of a <typeparamref name="T"/> to or from ISO 8601 string.
    /// </summary>
    /// <typeparam name="T">Converting type.</typeparam>
    public abstract class IsoStringTypeConverter<T> : TypeConverter
    {
        /// <summary>
        /// Converts an ISO 8601 string to an instance of a <typeparamref name="T"/>.
        /// </summary>
        /// <param name="s">ISO 8601 string to convert.</param>
        /// <returns>Instance of a <typeparamref name="T"/>.</returns>
        protected abstract T Parse(string s);

        /// <summary>
        /// Converts an instance of a <typeparamref name="T"/> to ISO 8601 string.
        /// </summary>
        /// <param name="source">Instance of a <typeparamref name="T"/>.</param>
        /// <returns>ISO 8601 string.</returns>
        protected abstract string ToIsoString(T source);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                return Parse(str);
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is T typedValue)
            {
                return ToIsoString(typedValue);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

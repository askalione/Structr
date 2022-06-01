using System;

namespace Structr.Security.Internal
{
    internal static class StringExtensions
    {
        public static bool TryCast<T>(this string src, out T value)
        {
            value = default(T);
            try
            {
                value = (T)Cast(src, typeof(T));
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static object Cast(this string src, Type type)
        {
            if (type == typeof(string))
            {
                return src;
            }

            if (string.IsNullOrWhiteSpace(src))
            {
                return null;
            }

            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsEnum)
            {
                return Enum.Parse(type, src);
            }

            var value = Convert.ChangeType(src, type);
            return value;
        }
    }
}

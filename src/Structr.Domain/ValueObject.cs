using System;
using System.Linq;
using System.Reflection;

namespace Structr.Domain
{
    /// <summary>
    /// General class for a value object <see cref="TValueObject"/>.
    /// </summary>
    /// <typeparam name="TValueObject">Type of value object.</typeparam>
    /// <remarks>
    /// Taken from https://github.com/cesarcastrocuba/nlayerappv3/blob/master/Domain.Seedwork/ValueObject.cs
    /// </remarks>
    public class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        public virtual bool Equals(TValueObject other)
        {
            if ((object)other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // Compare all public properties.
            PropertyInfo[] publicProperties = GetType().GetProperties();

            if (publicProperties != null && publicProperties.Any())
            {
                bool result = publicProperties.All(p =>
                {
                    object left = p.GetValue(this, null);
                    object right = p.GetValue(other, null);

                    if (left != null && typeof(TValueObject).IsAssignableFrom(left.GetType()))
                    {
                        // Check not self-references.
                        return ReferenceEquals(left, right);
                    }
                    else
                    {
                        return Equals(left, right);
                    }
                });
                return result;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            ValueObject<TValueObject> item = obj as ValueObject<TValueObject>;

            if ((object)item != null)
            {
                return Equals((TValueObject)item);
            }
            else
            {
                return false;
            }

        }

        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            // Compare all public properties.
            PropertyInfo[] publicProperties = GetType().GetProperties();

            if (publicProperties != null && publicProperties.Any())
            {
                foreach (PropertyInfo item in publicProperties)
                {
                    object value = item.GetValue(this, null);

                    if (value != null)
                    {
                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                    {
                        // Only for support { "a", null, null, "a" } <> { null, "a", "a", null }.
                        hashCode = hashCode ^ (index * 13);
                    }
                }
            }

            return hashCode;
        }

        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }
            else
            {
                return left.Equals(right);
            }
        }

        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !(left == right);
        }
    }
}

using System;
using System.Linq;
using System.Reflection;

namespace Structr.Domain
{
    /// <remarks>
    /// Taken from https://github.com/cesarcastrocuba/nlayerappv3/blob/master/Domain.Seedwork/ValueObject.cs
    /// </remarks>
    public class ValueObject<TValueObject> : IEquatable<TValueObject>
        where TValueObject : ValueObject<TValueObject>
    {
        public virtual bool Equals(TValueObject other)
        {
            if ((object)other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            //compare all public properties
            PropertyInfo[] publicProperties = this.GetType().GetProperties();

            if ((object)publicProperties != null
                &&
                publicProperties.Any())
            {
                return publicProperties.All(p =>
                {
                    var left = p.GetValue(this, null);
                    var right = p.GetValue(other, null);


                    if (typeof(TValueObject).IsAssignableFrom(left.GetType()))
                    {
                        //check not self-references...
                        return object.ReferenceEquals(left, right);
                    }
                    else
                        return left.Equals(right);


                });
            }
            else
                return true;
        }
        public override bool Equals(object obj)
        {
            if ((object)obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            ValueObject<TValueObject> item = obj as ValueObject<TValueObject>;

            if ((object)item != null)
                return Equals((TValueObject)item);
            else
                return false;

        }
        public override int GetHashCode()
        {
            int hashCode = 31;
            bool changeMultiplier = false;
            int index = 1;

            //compare all public properties
            PropertyInfo[] publicProperties = this.GetType().GetProperties();

            if ((object)publicProperties != null && publicProperties.Any())
            {
                foreach (var item in publicProperties)
                {
                    object value = item.GetValue(this, null);

                    if ((object)value != null)
                    {

                        hashCode = hashCode * ((changeMultiplier) ? 59 : 114) + value.GetHashCode();

                        changeMultiplier = !changeMultiplier;
                    }
                    else
                        hashCode = hashCode ^ (index * 13);//only for support {"a",null,null,"a"} <> {null,"a","a",null}
                }
            }

            return hashCode;
        }

        public static bool operator ==(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            if (object.Equals(left, null))
                return (object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);

        }

        public static bool operator !=(ValueObject<TValueObject> left, ValueObject<TValueObject> right)
        {
            return !(left == right);
        }
    }
}

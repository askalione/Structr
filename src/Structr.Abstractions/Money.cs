using Structr.Abstractions.Extensions;
using System;
using System.Collections.Generic;

namespace Structr.Abstractions
{
    public class Money<TValue> : IEquatable<Money<TValue>>, IComparable, IComparable<Money<TValue>>
        where TValue : struct, IComparable, IComparable<TValue>
    {
        public TValue Value { get; protected set; }

        protected Money() { }

        public Money(TValue value) : this()
        {
            Value = value;
        }

        #region IEquatable

        public bool Equals(Money<TValue> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Money<TValue>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (Value.Equals(default(TValue)))
                    return base.GetHashCode();

                return GetType().GetHashCode() ^ Value.GetHashCode();
            }
        }

        #endregion

        #region IComparable

        public int CompareTo(Money<TValue> other)
        {
            if (ReferenceEquals(null, other))
                return 1;

            return Value.CompareTo(other.Value);
        }

        public virtual int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;

            if (ReferenceEquals(this, obj))
                return 0;

            if (obj.GetType() != GetType())
                throw new InvalidOperationException(string.Format("Cannot convert object of type '{0}' to '{1}'.", obj.GetType().FullName, GetType().FullName));

            return CompareTo((Money<TValue>)obj);
        }

        #endregion

        #region Operators

        public static bool operator ==(Money<TValue> money1, Money<TValue> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return false;

            return money1.Equals(money2);
        }

        public static bool operator !=(Money<TValue> money1, Money<TValue> money2)
        {
            return !(money1 == money2);
        }

        public static bool operator >(Money<TValue> money1, Money<TValue> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return true;

            return money1.CompareTo(money2) > 0;
        }

        public static bool operator >=(Money<TValue> money1, Money<TValue> money2)
        {
            return (money1 > money2 || money1 == money2);
        }

        public static bool operator <(Money<TValue> money1, Money<TValue> money2)
        {
            return !(money1 >= money2);
        }

        public static bool operator <=(Money<TValue> money1, Money<TValue> money2)
        {
            return !(money1 > money2);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0:N2}", Value);
        }
    }

    public class Money<TValue, TCurrency> : Money<TValue>, IEquatable<Money<TValue, TCurrency>>, IComparable, IComparable<Money<TValue, TCurrency>>
        where TValue : struct, IComparable, IComparable<TValue>
        where TCurrency : struct
    {
        public TCurrency Currency { get; protected set; }

        protected Money() : base() { }

        public Money(TValue value, TCurrency currency) : base(value)
        {
            Currency = currency;
        }

        #region IEquatable

        public bool Equals(Money<TValue, TCurrency> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return EqualityComparer<TValue>.Default.Equals(Value, other.Value) &&
                string.Equals(Currency, other.Currency);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Money<TValue, TCurrency>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TValue>.Default.GetHashCode(Value) * 397) ^ Currency.GetHashCode();
            }
        }

        #endregion

        #region IComparable

        public int CompareTo(Money<TValue, TCurrency> other)
        {
            if (ReferenceEquals(null, other))
                return 1;

            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException(string.Format("Cannot compare {0} and {1}.", Currency, other.Currency));

            return Value.CompareTo(other.Value);
        }

        public override int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;

            if (ReferenceEquals(this, obj))
                return 0;

            if (obj.GetType() != GetType())
                throw new InvalidOperationException(string.Format("Cannot convert object of type '{0}' to '{1}'.", obj.GetType().FullName, GetType().FullName));

            return CompareTo(obj as Money<TValue, TCurrency>);
        }

        #endregion

        #region Operators

        public static bool operator ==(Money<TValue, TCurrency> money1, Money<TValue, TCurrency> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return false;

            return money1.Equals(money2);
        }

        public static bool operator !=(Money<TValue, TCurrency> money1, Money<TValue, TCurrency> money2)
        {
            return !(money1 == money2);
        }

        public static bool operator >(Money<TValue, TCurrency> money1, Money<TValue, TCurrency> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return true;

            return money1.CompareTo(money2) > 0;
        }

        public static bool operator >=(Money<TValue, TCurrency> money1, Money<TValue, TCurrency> money2)
        {
            return (money1 > money2 || money1 == money2);
        }

        public static bool operator <(Money<TValue, TCurrency> money1, Money<TValue, TCurrency> money2)
        {
            return !(money1 >= money2);
        }

        public static bool operator <=(Money<TValue, TCurrency> money1, Money<TValue, TCurrency> money2)
        {
            return !(money1 > money2);
        }

        #endregion

        public override string ToString()
        {
            string currency = Currency.ToString();
            if (Currency is Enum enumCurrency)
                currency = enumCurrency.GetDescription();
            return string.Format("{0:N2}", Value) + " " + currency;
        }
    }
}

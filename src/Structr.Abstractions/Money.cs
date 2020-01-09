using Structr.Abstractions.Extensions;
using System;
using System.Collections.Generic;

namespace Structr.Abstractions
{
    public class Money<TAmount> : IEquatable<Money<TAmount>>, IComparable, IComparable<Money<TAmount>>
        where TAmount : struct, IComparable, IComparable<TAmount>
    {
        public TAmount Amount { get; protected set; }

        protected Money() { }

        public Money(TAmount amount) : this()
        {
            Amount = amount;
        }

        #region IEquatable

        public bool Equals(Money<TAmount> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return EqualityComparer<TAmount>.Default.Equals(this.Amount, other.Amount);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Money<TAmount>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (Amount.Equals(default(TAmount)))
                    return base.GetHashCode();

                return GetType().GetHashCode() ^ Amount.GetHashCode();
            }
        }

        #endregion

        #region IComparable

        public int CompareTo(Money<TAmount> other)
        {
            if (ReferenceEquals(null, other))
                return 1;

            return Amount.CompareTo(other.Amount);
        }

        public virtual int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;

            if (ReferenceEquals(this, obj))
                return 0;

            if (obj.GetType() != GetType())
                throw new InvalidOperationException(string.Format("Cannot convert object of type '{0}' to '{1}'.", obj.GetType().FullName, GetType().FullName));

            return CompareTo((Money<TAmount>)obj);
        }

        #endregion

        #region Operators

        public static bool operator ==(Money<TAmount> money1, Money<TAmount> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return false;

            return money1.Equals(money2);
        }

        public static bool operator !=(Money<TAmount> money1, Money<TAmount> money2)
        {
            return !(money1 == money2);
        }

        public static bool operator >(Money<TAmount> money1, Money<TAmount> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return true;

            return money1.CompareTo(money2) > 0;
        }

        public static bool operator >=(Money<TAmount> money1, Money<TAmount> money2)
        {
            return (money1 > money2 || money1 == money2);
        }

        public static bool operator <(Money<TAmount> money1, Money<TAmount> money2)
        {
            return !(money1 >= money2);
        }

        public static bool operator <=(Money<TAmount> money1, Money<TAmount> money2)
        {
            return !(money1 > money2);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0:N2}", Amount);
        }
    }

    public class Money<TAmount, TCurrency> : Money<TAmount>, IEquatable<Money<TAmount, TCurrency>>, IComparable, IComparable<Money<TAmount, TCurrency>>
        where TAmount : struct, IComparable, IComparable<TAmount>
        where TCurrency : struct
    {
        public TCurrency Currency { get; protected set; }

        protected Money() : base() { }

        public Money(TAmount amount, TCurrency currency) : base(amount)
        {
            Currency = currency;
        }

        #region IEquatable

        public bool Equals(Money<TAmount, TCurrency> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return EqualityComparer<TAmount>.Default.Equals(Amount, other.Amount) &&
                string.Equals(Currency, other.Currency);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Money<TAmount, TCurrency>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TAmount>.Default.GetHashCode(Amount) * 397) ^ Currency.GetHashCode();
            }
        }

        #endregion

        #region IComparable

        public int CompareTo(Money<TAmount, TCurrency> other)
        {
            if (ReferenceEquals(null, other))
                return 1;

            if (!Currency.Equals(other.Currency))
                throw new InvalidOperationException(string.Format("Cannot compare {0} and {1}.", Currency, other.Currency));

            return Amount.CompareTo(other.Amount);
        }

        public override int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
                return 1;

            if (ReferenceEquals(this, obj))
                return 0;

            if (obj.GetType() != GetType())
                throw new InvalidOperationException(string.Format("Cannot convert object of type '{0}' to '{1}'.", obj.GetType().FullName, GetType().FullName));

            return CompareTo(obj as Money<TAmount, TCurrency>);
        }

        #endregion

        #region Operators

        public static bool operator ==(Money<TAmount, TCurrency> money1, Money<TAmount, TCurrency> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return false;

            return money1.Equals(money2);
        }

        public static bool operator !=(Money<TAmount, TCurrency> money1, Money<TAmount, TCurrency> money2)
        {
            return !(money1 == money2);
        }

        public static bool operator >(Money<TAmount, TCurrency> money1, Money<TAmount, TCurrency> money2)
        {
            if (ReferenceEquals(null, money1))
                return false;
            if (ReferenceEquals(null, money2))
                return true;

            return money1.CompareTo(money2) > 0;
        }

        public static bool operator >=(Money<TAmount, TCurrency> money1, Money<TAmount, TCurrency> money2)
        {
            return (money1 > money2 || money1 == money2);
        }

        public static bool operator <(Money<TAmount, TCurrency> money1, Money<TAmount, TCurrency> money2)
        {
            return !(money1 >= money2);
        }

        public static bool operator <=(Money<TAmount, TCurrency> money1, Money<TAmount, TCurrency> money2)
        {
            return !(money1 > money2);
        }

        #endregion

        public override string ToString()
        {
            string currency = Currency.ToString();
            if (Currency is Enum enumCurrency)
                currency = enumCurrency.GetDescription();
            return string.Format("{0:N2}", Amount) + " " + currency;
        }
    }
}

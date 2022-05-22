using System;
using System.Threading.Tasks;

namespace Structr.Operations
{
    /// <summary>
    /// Represents a void type, since <see cref="System.Void"/> is not a valid return type in C#.
    /// </summary>
    public readonly struct VoidResult : IEquatable<VoidResult>, IComparable<VoidResult>, IComparable
    {
        private static readonly VoidResult _value = new VoidResult();

        /// <summary>
        /// Default and only value of the <see cref="VoidResult"/> type.
        /// </summary>
        public static ref readonly VoidResult Value => ref _value;

        /// <summary>
        /// Task from a <see cref="VoidResult"/> type.
        /// </summary>
        public static readonly Task<VoidResult> TaskValue = Task.FromResult(_value);

        public bool Equals(VoidResult other) => true;

        public override bool Equals(object obj) => obj is VoidResult;

        public int CompareTo(VoidResult other) => 0;

        public int CompareTo(object obj) => obj is VoidResult ? 0 : throw new InvalidOperationException();

        public override int GetHashCode() => 0;

        public static bool operator ==(VoidResult left, VoidResult right) => true;

        public static bool operator !=(VoidResult left, VoidResult right) => false;

        public override string ToString() => "";
    }
}

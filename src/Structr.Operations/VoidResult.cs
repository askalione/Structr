using System;
using System.Threading.Tasks;

namespace Structr.Operations
{
    public readonly struct VoidResult : IEquatable<VoidResult>, IComparable<VoidResult>, IComparable
    {
        private static readonly VoidResult _value = new VoidResult();

        public static ref readonly VoidResult Value => ref _value;

        public static readonly Task<VoidResult> TaskValue = Task.FromResult(_value);

        public bool Equals(VoidResult other) => true;

        public int CompareTo(VoidResult other) => 0;

        public int CompareTo(object obj) => 0;

        public override int GetHashCode() => 0;

        public override bool Equals(object obj) => obj is VoidResult;

        public static bool operator ==(VoidResult left, VoidResult right) => true;

        public static bool operator !=(VoidResult left, VoidResult right) => false;

        public override string ToString() => "";
    }
}

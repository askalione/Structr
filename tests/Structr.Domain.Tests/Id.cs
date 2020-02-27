using System;

namespace Structr.Domain.Tests
{
    public class Id : IEquatable<Id>
    {
        public int Id1 { get; }
        public int Id2 { get; }

        public Id(int id1, int id2)
        {
            Id1 = id1;
            Id2 = id2;
        }

        public override bool Equals(object obj)
            => Equals(obj as Id);

        public bool Equals(Id other)
        {
            if (other == null)
                return false;

            return Id1 == other.Id1 && Id2 == other.Id2;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Id1.GetHashCode();
                hash = hash * 31 + Id2.GetHashCode();
                return hash;
            }
        }
    }
}

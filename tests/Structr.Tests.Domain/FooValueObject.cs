using Structr.Domain;
using System;

namespace Structr.Tests.Domain
{
    public class FooValueObject : ValueObject<FooValueObject>
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public decimal Weight { get; set; }

        public FooValueObject() { }

        public FooValueObject(FooValueObject valueObject) : this()
        {
            if (valueObject == null)
                throw new ArgumentNullException(nameof(valueObject));

            Name = valueObject.Name;
            Type = valueObject.Type;
            Weight = valueObject.Weight;
        }
    }
}

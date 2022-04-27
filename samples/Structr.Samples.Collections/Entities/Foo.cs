using Structr.Samples.Collections.Infrastructure;
using System;

namespace Structr.Samples.Collections.Entities
{
    public class Foo : Writable
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Currency Currency { get; private set; }
        public decimal Amount { get; private set; }

        public Foo(string name, Currency currency, decimal amount)
        {
            Id = Guid.NewGuid();
            Name = name;
            Currency = currency;
            Amount = amount;
        }
    }
}

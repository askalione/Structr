using Structr.Samples.Collections.Infrastructure;
using System;

namespace Structr.Samples.Collections.Dto
{
    public class FooDto : Writable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
    }
}

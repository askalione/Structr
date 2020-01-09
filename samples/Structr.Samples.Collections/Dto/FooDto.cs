using Structr.Samples.Collections.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Collections.Dto
{
    public class FooDto : Writable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
    }
}

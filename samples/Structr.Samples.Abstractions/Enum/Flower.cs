using System;

namespace Structr.Samples.Abstractions.Enum
{
    public class Flower
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EFlowerColor Color { get; set; }
        public DateTime? DateCreated { get; set; }

        private Flower() { }
    }
}

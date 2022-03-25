using System;

namespace Structr.Samples.Abstractions.Flowers
{
    public class Flower
    {
        public FlowerId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FlowerColor Color { get; set; }
        public DateTime? DateCreated { get; set; }

        private Flower() { }
    }
}

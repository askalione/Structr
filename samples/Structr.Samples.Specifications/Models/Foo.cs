using System;

namespace Structr.Samples.Specifications.Models
{
    public class Foo
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public int Age { get; set; }
        public DateTime? DateDeleted { get; set; }

        public override string ToString()
        {
            return $"Name={Name};Color={Color};Age={Age};DateDeleted={DateDeleted}";
        }
    }
}

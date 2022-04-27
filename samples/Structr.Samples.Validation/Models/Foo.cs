namespace Structr.Samples.Validation.Models
{
    public enum Color
    {
        White,
        Blue,
        Red
    }

    public abstract class Foo
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public Color Color { get; set; }
    }
}

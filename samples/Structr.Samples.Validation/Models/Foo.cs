namespace Structr.Samples.Validation.Models
{
    public enum EColor
    {
        White,
        Blue,
        Red
    }

    public abstract class Foo
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public EColor Color { get; set; }
    }
}

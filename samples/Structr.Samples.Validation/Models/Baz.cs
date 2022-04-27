namespace Structr.Samples.Validation.Models
{
    public enum Shape
    {
        Square,
        Сircle,
        Triangle
    }

    public class Baz : Foo
    {
        public Shape Shape { get; set; }
    }
}

namespace Structr.Samples.Validation.Models
{
    public enum EShape
    {
        Square,
        Сircle,
        Triangle
    }

    public class Baz : Foo
    {
        public EShape Shape { get; set; }
    }
}

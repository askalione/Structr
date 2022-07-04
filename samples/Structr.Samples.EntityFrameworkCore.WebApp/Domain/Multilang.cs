using Structr.Abstractions;
using Structr.Domain;
using Structr.Samples.EntityFrameworkCore.WebApp.Utils.Extensions;

namespace Structr.Samples.EntityFrameworkCore.WebApp.Domain
{
    public class Multilang : ValueObject<Multilang>
    {
        public string? Ru { get; set; }
        public string? En { get; set; }

        public Multilang()
        { }

        public Multilang(string value)
        {
            Ensure.NotEmpty(value, nameof(value));

            if (value.IsCyrillic())
            {
                Ru = value;
            }
            else
            {
                En = value;
            }
        }

        public bool HasAnyValue()
            => string.IsNullOrEmpty(Ru) == false || string.IsNullOrEmpty(En) == false;

        public bool HasAllValues()
            => string.IsNullOrEmpty(Ru) == false && string.IsNullOrEmpty(En) == false;

        public void EnsureHasAnyValue(string name)
        {
            if (HasAnyValue() == false)
            {
                throw new ArgumentException($"{name} must contain {nameof(Ru)} or {nameof(En)} value.", name);
            }
        }

        public void EnsureHasAllValues(string name)
        {
            if (HasAllValues() == false)
            {
                throw new ArgumentException($"{name} must contain {nameof(Ru)} and {nameof(En)} value.", name);
            }
        }
    }
}

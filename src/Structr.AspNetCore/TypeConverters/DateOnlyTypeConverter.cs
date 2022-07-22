using System;

namespace Structr.AspNetCore.TypeConverters
{
    public sealed class DateOnlyTypeConverter : IsoStringTypeConverter<DateOnly>
    {
        protected override DateOnly Parse(string s) => DateOnly.Parse(s);

        protected override string ToIsoString(DateOnly source) => source.ToString("O");
    }
}

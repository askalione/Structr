using System;

namespace Structr.AspNetCore.TypeConverters
{
    public sealed class TimeOnlyTypeConverter : IsoStringTypeConverter<TimeOnly>
    {
        protected override TimeOnly Parse(string s) => TimeOnly.Parse(s);

        protected override string ToIsoString(TimeOnly source) => source.ToString("O");
    }
}

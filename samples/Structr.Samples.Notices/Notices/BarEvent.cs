using Structr.Notices;
using System;

namespace Structr.Samples.Notices.Notices
{
    public class BarEvent : INotice
    {
        public DateTime Date { get; set; }
    }
}

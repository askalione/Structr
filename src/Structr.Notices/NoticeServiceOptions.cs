using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Notices
{
    public class NoticeServiceOptions
    {
        public Type PublisherType { get; set; }
        public ServiceLifetime Lifetime { get; set; }

        public NoticeServiceOptions()
        {
            PublisherType = typeof(NoticePublisher);
            Lifetime = ServiceLifetime.Scoped;
        }
    }
}

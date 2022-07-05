using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Notices
{
    /// <summary>
    /// Defines a set of options used for configure services.
    /// </summary>
    public class NoticeServiceOptions
    {
        /// <summary>
        /// Determines a type of notice publisher. The <see cref="NoticePublisher"/> by default.
        /// </summary>
        public Type PublisherServiceType { get; set; }

        /// <summary>
        /// Specifies the lifetime of a notice service in an <see cref="IServiceCollection"/>. The <see cref="ServiceLifetime.Scoped"/> by default.
        /// </summary>
        public ServiceLifetime PublisherServiceLifetime { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="NoticeServiceOptions"/> with default values.
        /// </summary>
        public NoticeServiceOptions()
        {
            PublisherServiceType = typeof(NoticePublisher);
            PublisherServiceLifetime = ServiceLifetime.Transient;
        }
    }
}

using System;

namespace Structr.Configuration
{
    public interface IConfigurator<TSettings> where TSettings : class, new()
    {
        void Configure(Action<TSettings> changes);
    }
}

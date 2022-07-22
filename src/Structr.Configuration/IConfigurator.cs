using System;

namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for configure a settings <typeparamref name="TSettings"/>.
    /// </summary>
    /// <typeparam name="TSettings">Settings class.</typeparam>
    public interface IConfigurator<TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Configure a settings <typeparamref name="TSettings"/>.
        /// </summary>
        /// <param name="changes">Action for configure a settings <typeparamref name="TSettings"/>.</param>
        void Configure(Action<TSettings> changes);
    }
}

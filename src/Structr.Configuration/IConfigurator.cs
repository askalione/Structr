using System;

namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for configure a settings <see cref="TSettings"/>.
    /// </summary>
    /// <typeparam name="TSettings">Settings class.</typeparam>
    public interface IConfigurator<TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Configure a settings <see cref="TSettings"/>.
        /// </summary>
        /// <param name="changes">Action for configure a settings <see cref="TSettings"/>.</param>
        void Configure(Action<TSettings> changes);
    }
}

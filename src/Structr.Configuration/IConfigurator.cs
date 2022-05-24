using System;

namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for congigure a settings <see cref="TSettings"/>.
    /// </summary>
    /// <typeparam name="TSettings">Settings class.</typeparam>
    public interface IConfigurator<TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Congigure a settings <see cref="TSettings"/>.
        /// </summary>
        /// <param name="changes">Action for congigure a settings <see cref="TSettings"/>.</param>
        void Configure(Action<TSettings> changes);
    }
}

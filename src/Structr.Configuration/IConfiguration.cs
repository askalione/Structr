namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for accessing to an application settings contained in instance of <typeparamref name="TSettings"/>.
    /// </summary>
    /// <typeparam name="TSettings">Settings class.</typeparam>
    public interface IConfiguration<out TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Returns an instance of <typeparamref name="TSettings"/> containing application settings.
        /// </summary>
        TSettings Settings { get; }
    }
}

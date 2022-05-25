namespace Structr.Configuration
{
    /// <summary>
    /// Provides functionality for access to a settings <see cref="TSettings"/>.
    /// </summary>
    /// <typeparam name="TSettings">Settings class.</typeparam>
    public interface IConfiguration<out TSettings> where TSettings : class, new()
    {
        /// <summary>
        /// Returns a settings <see cref="TSettings"/>.
        /// </summary>
        TSettings Settings { get; }
    }
}

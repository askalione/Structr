using Newtonsoft.Json;
using Structr.Configuration.Json;
using System;
using System.IO;

namespace Structr.Configuration.Providers
{
    /// <summary>
    /// Provides functionality for access to a file with settings <see cref="TSettings"/>.
    /// </summary>
    /// <inheritdoc cref="SettingsProvider{TSettings}"/>
    public abstract class FileSettingsProvider<TSettings> : SettingsProvider<TSettings> where TSettings : class, new()
    {
        protected readonly string Path;
        private DateTime? _lastModifiedTime;

        protected JsonSerializer JsonSerializer { get; }

        /// <summary>
        /// Initializes a new <see cref="FileSettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="options">The <see cref="SettingsProviderOptions"/>.</param>
        /// <param name="path">The path to file with settings.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="path"/> is <see langword="null"/> or empty.</exception>
        public FileSettingsProvider(SettingsProviderOptions options, string path) : base(options)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Path = path;
            JsonSerializer = new JsonSerializer
            {
                ContractResolver = new JsonSettingsContractResolver()
            };
        }

        protected override bool IsSettingsModified()
        {
            ValidatePathOrThrow();

            var fileInfo = new FileInfo(Path);
            var lastModifiedTime = fileInfo.LastWriteTime;
            var isModified = _lastModifiedTime != lastModifiedTime;
            _lastModifiedTime = lastModifiedTime;

            return isModified;
        }

        protected void ValidatePathOrThrow()
        {
            if (File.Exists(Path) == false)
            {
                throw new FileNotFoundException($"Settings file not found.", Path);
            }
        }
    }
}

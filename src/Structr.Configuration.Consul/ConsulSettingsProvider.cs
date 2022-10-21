using Consul;
using Newtonsoft.Json;
using Structr.Configuration.Consul.Internal;
using Structr.Configuration.Json;
using System;
using System.IO;
using System.Text;

namespace Structr.Configuration.Consul
{
    /// <summary>
    /// Provides functionality for access to Consul KV store with settings <typeparamref name="TSettings"/>.
    /// </summary>
    public class ConsulSettingsProvider<TSettings> : SettingsProvider<TSettings>
        where TSettings : class, new()
    {
        private readonly string _key;
        private readonly IConsulClient _consulClient;
        private JsonSerializer _jsonSerializer { get; }

        private ulong? _lastModifyIndex;

        /// <summary>
        /// Initializes a new <see cref="ConsulSettingsProvider{TSettings}"/> instance.
        /// </summary>
        /// <param name="key">The Consul key.</param>
        /// <param name="consulClient">The <see cref="IConsulClient"/>.</param>
        /// <param name="options">The options object to make additional configurations.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="key"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="consulClient"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        public ConsulSettingsProvider(string key,
            IConsulClient consulClient,
            SettingsProviderOptions options)
            : base(options)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (consulClient == null)
            {
                throw new ArgumentNullException(nameof(consulClient));
            }

            _key = key;
            _consulClient = consulClient;
            _jsonSerializer = new JsonSerializer
            {
                ContractResolver = new JsonSettingsContractResolver()
            };
        }

        protected override bool IsSettingsModified()
        {
            KVPair kvPair = GetKVPair();
            bool isModified = kvPair.ModifyIndex != _lastModifyIndex;
            _lastModifyIndex = kvPair.ModifyIndex;

            return isModified;
        }

        protected override TSettings LoadSettings()
        {
            KVPair kvPair = GetKVPair();

            using StringReader stringReader = new StringReader(Encoding.UTF8.GetString(kvPair.Value, 0, kvPair.Value.Length));
            using JsonTextReader reader = new JsonTextReader(stringReader);
            TSettings settings = _jsonSerializer.Deserialize<TSettings>(reader);

            return settings;
        }

        protected override void LogFirstAccess()
        {
            KVPair kvPair = GetKVPair();
            _lastModifyIndex = kvPair.ModifyIndex;
        }

        protected override void UpdateSettings(TSettings settings)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);
            using JsonTextWriter writer = new JsonTextWriter(stringWriter);
            _jsonSerializer.Serialize(writer, settings);
            byte[] value = Encoding.UTF8.GetBytes(sb.ToString());
            var kvPair = new KVPair(_key) { Value = value };
            AddOrUpdateKVPair(kvPair);
        }

        private KVPair GetKVPair()
        {
            QueryResult<KVPair> queryResult = AsyncHelper.RunSync(() => _consulClient.KV.Get(_key));
            KVPair kvPair = queryResult?.Response;
            if (kvPair == null)
            {
                throw new NullReferenceException($"Settings with key \"{_key}\" not found.");
            }

            return kvPair;
        }

        private void AddOrUpdateKVPair(KVPair kvPair)
        {
            AsyncHelper.RunSync(() => _consulClient.KV.Put(kvPair));
        }
    }
}

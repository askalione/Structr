using System.Collections.Generic;

namespace Structr.Abstractions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds new values to source dictionary, overriding values for existing keys.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="source"></param>
        /// <param name="dictionary">Key-value pairs to be added to source dictionary.</param>
        public static void AddRangeOverride<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> dictionary)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(dictionary, nameof(dictionary));

            dictionary.ForEach(x => source[x.Key] = x.Value);
        }

        /// <summary>
        /// Adds new values to source dictionary, leaving existing keys values untouched.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="source"></param>
        /// <param name="dictionary">Key-value pairs to be added to source dictionary.</param>
        public static void AddRangeNewOnly<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> dictionary)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(dictionary, nameof(dictionary));

            dictionary.ForEach(x =>
            {
                if (source.ContainsKey(x.Key) == false)
                {
                    source.Add(x.Key, x.Value);
                }
            });
        }

        /// <summary>
        /// Adds new values to source dictionary.
        /// Throws if one or more keys in new list already exist in source dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="source"></param>
        /// <param name="dictionary">Key-value pairs to be added to source dictionary.</param>
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> dictionary)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(dictionary, nameof(dictionary));

            dictionary.ForEach(x => source.Add(x.Key, x.Value));
        }


        /// <summary>
        /// Checks if at least one key from provided list exists in source dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="source"></param>
        /// <param name="dictionary">Key-value pairs to be added to source dictionary.</param>
        public static bool ContainsKeys<TKey, TValue>(this Dictionary<TKey, TValue> source, IEnumerable<TKey> keys)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(keys, nameof(keys));

            bool result = false;
            keys.ForEachOrBreak((x) => { result = source.ContainsKey(x); return result; });
            return result;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Structr.Abstractions.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddRangeOverride<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> dictionary)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(dictionary, nameof(dictionary));

            dictionary.ForEach(x => source[x.Key] = x.Value);
        }

        public static void AddRangeNewOnly<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> dictionary)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(dictionary, nameof(dictionary));

            dictionary.ForEach(x => { if (!source.ContainsKey(x.Key)) source.Add(x.Key, x.Value); });
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> dictionary)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(dictionary, nameof(dictionary));

            dictionary.ForEach(x => source.Add(x.Key, x.Value));
        }

        public static bool ContainsKeys<TKey, TValue>(this Dictionary<TKey, TValue> source, IEnumerable<TKey> keys)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(keys, nameof(keys));

            bool result = false;
            keys.ForEachOrBreak((x) => { result = source.ContainsKey(x); return result; });
            return result;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(action, nameof(action));

            foreach (var item in source)
                action(item);
        }

        public static void ForEachOrBreak<T>(this IEnumerable<T> source, Func<T, bool> func)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(func, nameof(func));

            foreach (var item in source)
            {
                bool result = func(item);
                if (result) break;
            }
        }
    }
}

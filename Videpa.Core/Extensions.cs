using System;
using System.Collections.Generic;

namespace Videpa.Core
{
    public static partial class Extensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
                TKey key)
        {
            if (key == null)
                return default(TValue);

            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : value;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
                TKey key,
                TValue defaultValue)
        {
            if (key == null)
                return default(TValue);

            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
                TKey key,
                Func<TValue> defaultValueProvider)
        {
            if (key == null)
                return default(TValue);

            TValue value;
            return dictionary.TryGetValue(key, out value) ? value
                : defaultValueProvider();
        }


    }
}
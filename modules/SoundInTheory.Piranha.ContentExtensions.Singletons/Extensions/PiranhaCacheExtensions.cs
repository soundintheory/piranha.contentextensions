using Piranha;
using Piranha.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Extensions
{
    internal static class PiranhaCacheExtensions
    {
        internal static bool TryGet<T>(this ICache cache, string key, out T value)
        {
            if (cache == null)
            {
                value = default;
                return false;
            }

            value = cache.Get<T>(key);
            return !value.IsNullOrDefault();
        }

        internal static T GetOrAdd<T>(this ICache cache, string key, Func<T> factory)
        {
            if (cache == null)
            {
                return factory.Invoke();
            }

            if (!cache.TryGet<T>(key, out var value))
            {
                value = factory.Invoke();
                cache.Set(key, value);
            }

            return value;
        }

        internal async static Task<T> GetOrAddAsync<T>(this ICache cache, string key, Func<Task<T>> factory)
        {
            if (cache == null)
            {
                return await factory.Invoke();
            }

            if (!cache.TryGet<T>(key, out var value))
            {
                value = await factory.Invoke();
                cache.Set(key, value);
            }

            return value;
        }
    }
}

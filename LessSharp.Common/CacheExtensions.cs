using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LessSharp.Common
{
    public static class CacheExtensions
    {
        public static T GetObject<T>(this IDistributedCache cache, string key)
        {
            var json = cache.GetString(key);
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return CommonHelper.JsonStringToObject<T>(json);
        }
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key)
        {
            var json = await cache.GetStringAsync(key, default(CancellationToken));
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            return CommonHelper.JsonStringToObject<T>(json);
        }

        public static void SetObject(this IDistributedCache cache, string key, object obj, DistributedCacheEntryOptions options)
        {
            string json = CommonHelper.ObjectToJsonString(obj);
            cache.SetString(key, json, options);
        }

        public static void SetObject(this IDistributedCache cache, string key, object obj)
        {
            cache.SetObject(key, obj, new DistributedCacheEntryOptions());
        }

        public static async Task SetObjectAsync(this IDistributedCache cache, string key, object obj, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            string json = CommonHelper.ObjectToJsonString(obj);
            await cache.SetStringAsync(key, json, options, token);
        }

        public static async Task SetObjectAsync(this IDistributedCache cache, string key, object obj, CancellationToken token = default(CancellationToken))
        {
            await cache.SetObjectAsync(key, obj, new DistributedCacheEntryOptions(), token);
        }
    }
}

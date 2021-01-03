using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Common.CacheHelper
{
    /// <summary>
    /// 缓存助手
    /// </summary>
    public class MemoryCacheHelper : ICacheHelper
    {
        private readonly IMemoryCache _cache;
        public MemoryCacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool Exists(string key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }

        public void Set<T>(string key, T value)
        {
            _cache.Set(key, value);
        }
        public void Expire(string key, DateTime dateTime)
        {
            var value = _cache.Get(key);
            _cache.Set(key, value, dateTime);
        }

        public void Expire(string key, TimeSpan timeSpan)
        {
            var value = _cache.Get(key);
            _cache.Set(key, value, timeSpan);
        }
        public void HashAdd(string key, string hashKey, object hashValue)
        {
            var hash = _cache.Get<Dictionary<string, object>>(key);
            if (hash == null)
            {
                hash = new Dictionary<string, object>();
            }
            if (hash.ContainsKey(hashKey))
            {
                hash[key] = hashValue;
            }
            else
            {
                hash.Add(hashKey, hashValue);
            }
            _cache.Set<Dictionary<string, object>>(key, hash);
        }

        public T HashGet<T>(string key, string hashKey)
        {
            var hash = _cache.Get<Dictionary<string, object>>(key);
            if (hash.ContainsKey(hashKey))
            {
                return (T)hash[hashKey];
            }
            else
            {
                return default(T);
            }
        }

        public bool HashExists(string key, string hashKey)
        {
            var hash = _cache.Get<Dictionary<string, object>>(key);
            return hash?.ContainsKey(hashKey) ?? false;
        }

        public void HashRemove(string key, string hashKey)
        {
            var hash = _cache.Get<Dictionary<string, object>>(key);
            if (hash.ContainsKey(hashKey))
            {
                hash.Remove(hashKey);
            }
        }

        public void SetAdd(string key, params string[] values)
        {
            var memoryValues = _cache.Get<HashSet<string>>(key) ?? new HashSet<string>();
            foreach (var item in values)
            {
                memoryValues.Add(item);
            }
            _cache.Set<HashSet<string>>(key, memoryValues);
        }

        public void SetRemove(string key, params string[] values)
        {
            var memoryValues = _cache.Get<HashSet<string>>(key);
            if (memoryValues != null)
            {
                foreach (var item in values)
                {
                    memoryValues.Remove(item);
                }
            }
        }

        public bool SetContains(string key, string value)
        {
            var memoryValues = _cache.Get<HashSet<string>>(key);
            return memoryValues?.Contains(value) ?? false;
        }

        public HashSet<string> SetGet(string key)
        {
            return _cache.Get<HashSet<string>>(key);
        }
    }
}

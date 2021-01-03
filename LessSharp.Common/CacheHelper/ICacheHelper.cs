using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Common.CacheHelper
{
    public interface ICacheHelper
    {
        bool Exists(string key);

        void Set<T>(string key, T value);

        T Get<T>(string key);

        void Delete(string key);


        void Expire(string key, DateTime dateTime);
        void Expire(string key, TimeSpan timeSpan);

        void HashAdd(string key, string hashKey, object hashValue);
        T HashGet<T>(string key, string hashKey);

        bool HashExists(string key, string hashKey);

        void HashRemove(string key, string hashKey);
        void SetAdd(string key, params string[] values);

        void SetRemove(string key, params string[] values);
        bool SetContains(string key, string value);
        HashSet<string> SetGet(string key);
    }
}

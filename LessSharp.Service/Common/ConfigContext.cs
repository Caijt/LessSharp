using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LessSharp.Common;
using LessSharp.Common.CacheHelper;
using LessSharp.Data;
using LessSharp.Entity.Sys;
using LessSharp.Service.Sys;
using Microsoft.Extensions.Caching.Distributed;

namespace LessSharp.Service.Common
{
    /// <summary>
    /// 配置中心
    /// </summary>
    public class ConfigContext
    {
        private readonly ConfigService _configService;
        public ConfigContext(ConfigService configService)
        {
            _configService = configService;
        }

        public string GetValue(ConfigKey key)
        {
            return _configService.GetValueByKey(key.ToString()).Result;
            //if (_cache.HashExists(ConfigCacheKey, key.ToString()))
            //{
            //    return _cache.HashGet<string>(ConfigCacheKey, key.ToString());
            //}
            //string value = _configService.GetValueByKey(key.ToString()).Result;
            //_cache.HashSet(ConfigCacheKey, key.ToString(), value);
            //return value;
        }

        public bool GetBoolValue(ConfigKey key)
        {
            return GetValue(key) == "ON" ? true : false;
        }

        public decimal GetNumberValue(ConfigKey key)
        {
            return decimal.Parse(GetValue(key));
        }
    }
}

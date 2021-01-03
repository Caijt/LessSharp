using AspectCore.DynamicProxy;
using LessSharp.Common.CacheHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LessSharp.Common;

namespace LessSharp.Service.Interceptors
{
    /// <summary>
    /// 缓存删除拦截器
    /// </summary>
    public class CacheDeleteInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly string[] _keys;

        public CacheDeleteInterceptorAttribute(params string[] keys)
        {
            _keys = keys;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cache = context.ServiceProvider.GetService<ICacheHelper>();
            await next(context);
            if (_keys != null)
            {
                foreach (var key in _keys)
                {
                    cache.Delete(key);
                }
            }
        }
    }
}

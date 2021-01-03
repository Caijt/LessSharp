using AspectCore.DynamicProxy;
using LessSharp.Common;
using LessSharp.Common.CacheHelper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LessSharp.Service.Interceptors
{
    /// <summary>
    /// 方法缓存删除拦截器
    /// </summary>
    public class MethodCacheDeleteInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly Type[] _types;
        private readonly string[] _methods;

        /// <summary>
        /// 需传入相同数量的Types跟Methods，同样位置的Type跟Method会组合成一个缓存key，进行删除
        /// </summary>
        /// <param name="Types">传入要删除缓存的类</param>
        /// <param name="Methods">传入要删除缓存的方法名称，必须与Types数组对应</param>
        public MethodCacheDeleteInterceptorAttribute(Type[] Types, string[] Methods)
        {
            if (Types.Length != Methods.Length)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "Types必须跟Methods数量一致");
            }
            _types = Types;
            _methods = Methods;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cache = context.ServiceProvider.GetService<ICacheHelper>();
            await next(context);
            for (int i = 0; i < _types.Length; i++)
            {
                var type = _types[i];
                var method = _methods[i];
                string key = "Methods:" + type.FullName + "." + method;
                cache.Delete(key);
            }
        }
    }
}

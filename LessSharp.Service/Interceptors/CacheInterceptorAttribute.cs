using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using LessSharp.Common;
using AspectCore.DynamicProxy.Parameters;
using LessSharp.Common.CacheHelper;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LessSharp.Service.Interceptors
{
    /// <summary>
    /// 缓存拦截器
    /// </summary>
    public class CacheInterceptorAttribute : AbstractInterceptorAttribute
    {
        public string CacheKey { get; set; }
        /// <summary>
        /// 缓存秒数
        /// </summary>
        public int ExpireSeconds { get; set; }
        public CacheInterceptorAttribute()
        {

        }
        public CacheInterceptorAttribute(string cacheKey)
        {
            CacheKey = cacheKey;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            //判断是否是异步方法
            bool isAsync = context.IsAsync();
            //if (context.ImplementationMethod.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
            //{
            //    isAsync = true;
            //}
            //先判断方法是否有返回值，无就不进行缓存判断
            var methodReturnType = context.GetReturnParameter().Type;
            if (methodReturnType == typeof(void) || methodReturnType == typeof(Task) || methodReturnType == typeof(ValueTask))
            {
                await next(context);
                return;
            }
            var returnType = methodReturnType;
            if (isAsync)
            {
                //取得异步返回的类型
                returnType = returnType.GenericTypeArguments.FirstOrDefault();
            }
            //获取方法参数名
            string param = CommonHelper.ObjectToJsonString(context.Parameters);

            string key = CacheKey;
            if (string.IsNullOrWhiteSpace(CacheKey))
            {
                //获取方法名称，也就是缓存key值
                key = "Methods:" + context.ImplementationMethod.DeclaringType.FullName + "." + context.ImplementationMethod.Name;
            }
            var cache = context.ServiceProvider.GetService<ICacheHelper>();
            //如果缓存有值，那就直接返回缓存值
            if (cache.HashExists(key, param))
            {
                //反射获取缓存值，相当于cache.HashGet<>(key,param)
                var value = typeof(ICacheHelper).GetMethod(nameof(ICacheHelper.HashGet)).MakeGenericMethod(returnType).Invoke(cache, new[] { key, param });
                if (isAsync)
                {
                    //判断是Task还是ValueTask
                    if (methodReturnType == typeof(Task<>).MakeGenericType(returnType))
                    {
                        //反射获取Task<>类型的返回值，相当于Task.FromResult(value)
                        context.ReturnValue = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(returnType).Invoke(null, new[] { value });
                    }
                    else if (methodReturnType == typeof(ValueTask<>).MakeGenericType(returnType))
                    {
                        //反射构建ValueTask<>类型的返回值，相当于new ValueTask(value)
                        context.ReturnValue = Activator.CreateInstance(typeof(ValueTask<>).MakeGenericType(returnType), value);
                    }
                }
                else
                {
                    context.ReturnValue = value;
                }
                return;
            }
            await next(context);
            object returnValue;
            if (isAsync)
            {
                returnValue = await context.UnwrapAsyncReturnValue();
                //反射获取异步结果的值，相当于(context.ReturnValue as Task<>).Result
                //returnValue = typeof(Task<>).MakeGenericType(returnType).GetProperty(nameof(Task<object>.Result)).GetValue(context.ReturnValue);

            }
            else
            {
                returnValue = context.ReturnValue;
            }
            cache.HashAdd(key, param, returnValue);
            if (ExpireSeconds > 0)
            {
                cache.Expire(key, TimeSpan.FromSeconds(ExpireSeconds));
            }

        }
    }
}

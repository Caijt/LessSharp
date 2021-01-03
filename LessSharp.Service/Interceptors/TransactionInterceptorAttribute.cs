using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using LessSharp.Common;
using LessSharp.Common.CacheHelper;
using LessSharp.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LessSharp.Service.Interceptors
{
    /// <summary>
    /// 事务拦截器
    /// </summary>
    public class TransactionInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var dbContext = context.ServiceProvider.GetService<AppDbContext>();
            //先判断是否已经启用了事务
            if (dbContext.Database.CurrentTransaction == null)
            {
                await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await next(context);
                    dbContext.Database.CommitTransaction();
                }
                catch (Exception ex)
                {
                    dbContext.Database.RollbackTransaction();
                    throw ex;
                }
            }
            else
            {
                await next(context);
            }
        }
    }

    

    
}

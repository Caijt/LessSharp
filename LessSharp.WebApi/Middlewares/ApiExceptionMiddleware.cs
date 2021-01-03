using LessSharp.Common;
using LessSharp.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace LessSharp.WebApi.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var type = ex.TargetSite?.DeclaringType;
                var loggerName = context.Request.Path + (type != null ? $"[{type?.FullName}]" : "");
                var logger = loggerFactory.CreateLogger(loggerName);

                ApiResultDto resultDto = null;
                //判断是否是ApiFailException异常类型
                if (ex is ApiFailException)
                {
                    var apiException = ex as ApiFailException;
                    resultDto = ApiResultDto.Fail(apiException.Code, apiException.Message);
                    logger.LogWarning(resultDto.Message);
                }
                else
                {
                    //异常处理
                    var exceptionId = Guid.NewGuid();
                    resultDto = ApiResultDto.Exception(GetExceptionMessage(ex), exceptionId);
                    var eventId = new EventId(resultDto.Code, exceptionId.ToString());
                    logger.LogError(eventId, ex, resultDto.Message + "[" + exceptionId.ToString() + "]");
                }
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(CommonHelper.ObjectToJsonString(resultDto));
            }
        }

        private string GetExceptionMessage(Exception exception)
        {
            if (exception.InnerException == null)
            {
                return exception.Message;
            }
            return GetExceptionMessage(exception.InnerException);
        }
    }
}

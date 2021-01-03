using LessSharp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LessSharp.Dto
{
    /// <summary>
    /// Api执行结果模型
    /// </summary>
    public class ApiResultDto
    {
        /// <summary>
        /// Api接口执行代码
        /// </summary>
        public int Code { get; set; } = 0;
        /// <summary>
        /// Api执行结果描述
        /// </summary>
        public string Message { get; set; } = "ok";
        public ApiResultDto()
        { }
        public static ApiResultDto Success()
        {
            return new ApiResultDto();
        }
        public static ApiResultDto Fail(ApiFailCode code,string message = null)
        {
            var result = new ApiResultDto
            {
                Code = (int)code
            };
            if (string.IsNullOrWhiteSpace(message))
            {
                var attributes = code.GetType().GetField(code.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    var attribute = attributes[0] as DescriptionAttribute;
                    result.Message = attribute.Description;
                }
            }
            else {
                result.Message = message;
            }            
            return result;
        }
        public static ApiResultDto Exception(string message)
        {
            return new ApiResultDto
            {
                Code = -1,
                Message = message
            };
        }
        public static ApiResultDto Exception(string message, Guid exceptionId)
        {
            return new ApiResultDto<Guid>
            {
                Code = -1,
                Message = message,
                Data = exceptionId
            };
        }
    }
    /// <summary>
    /// Api执行结果模型（带有数据）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResultDto<T> : ApiResultDto
    {
        /// <summary>
        /// Api返回数据
        /// </summary>
        public T Data { get; set; }
        public static implicit operator ApiResultDto<T>(T data)
        {
            return new ApiResultDto<T>
            {
                Data = data
            };
        }
        public static ApiResultDto<T> Success(T data)
        {
            return data;
        }

        public static ApiResultDto<T> Fail(ApiFailCode code, T data)
        {
            var result = new ApiResultDto<T>
            {
                Code = (int)code,
                Data = data
            };
            var attributes = code.GetType().GetField(code.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                var attribute = attributes[0] as DescriptionAttribute;
                result.Message = attribute.Description;
            }
            return result;
        }
    }
}

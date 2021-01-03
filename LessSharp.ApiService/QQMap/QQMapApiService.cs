using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LessSharp.ApiService.QQMap.Dto;
using LessSharp.Common;
using LessSharp.Option;
namespace LessSharp.ApiService.QQMap
{
    public class QQMapApiService
    {
        private readonly HttpClient _httpClient;
        private readonly KeyOption _qyWeixinOption;
        private readonly IMemoryCache _memoryCache;
        public QQMapApiService(HttpClient httpClient, IOptionsSnapshot<KeyOption> option, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _qyWeixinOption = option.Get("QQMap");
            _memoryCache = memoryCache;
            httpClient.BaseAddress = new Uri("https://apis.map.qq.com/ws/");

        }

        /// <summary>
        /// 统一请求方法，会根据返回错误码对应操作或者抛出异常，正常就返回对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<T> RequestAsync<T>(string url) where T : ApiResult
        {
            var res = await _httpClient.GetAsync(url);
            var result = await res.Content.ReadAsAsync<T>();
            if (result.Status == (int)ErrorCode.SUCCESS)
            {
                return result;
            }
            string message = $"QQMapApi接口错误：[{result.Status}:{result.Message}]";
            if (Enum.IsDefined(typeof(ErrorCode), result.Status))
            {
                var errorCode = (ErrorCode)result.Status;
                var attributes = errorCode.GetType().GetField(errorCode.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    var attribute = attributes[0] as DescriptionAttribute;
                    message = attribute.Description;
                }
            }
            throw new ApiFailException(ApiFailCode.API_FAIL, message);
        }

        public async Task<Address> LocationToAddress(decimal lat, decimal lng)
        {
            string url = $"geocoder/v1/?location={lat},{lng}&key={_qyWeixinOption.Key}";
            return await this.RequestAsync<Address>(url);
        }
    }

    public enum ErrorCode
    {
        [Description("系统繁忙")]
        SYS_BUSY = -1,
        SUCCESS = 0,
        [Description("请求来源未被授权")]
        FAIL_REQUEST = 110,
        [Description("请求参数信息有误")]
        FAIL_PARAMS = 310,
        [Description("Key格式错误")]
        FAIL_KEY = 311,
        [Description("请求有护持信息请检查字符串")]
        FAIL_STR = 306,
    }
}

using Microsoft.Extensions.Caching.Distributed;
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
using LessSharp.ApiService.Qywx.Dto;
using LessSharp.Common;
using LessSharp.Dto;
using LessSharp.Option;
namespace LessSharp.ApiService.Qywx
{
    public class QywxApiService
    {
        private readonly HttpClient _httpClient;
        private readonly QyWeixinOption _qyWeixinOption;
        private readonly IDistributedCache _cache;
        private const string AccessTokenCacheKey = "Qywx:AccessToken";
        private const string JsapiTicketCacheKey = "Qywx:JsapiTicket";
        public QywxApiService(HttpClient httpClient, IOptionsSnapshot<QyWeixinOption> option, IDistributedCache cache)
        {
            _httpClient = httpClient;
            _qyWeixinOption = option.Value;
            _cache = cache;
            httpClient.BaseAddress = new Uri("https://qyapi.weixin.qq.com/cgi-bin/");

        }

        /// <summary>
        /// 获取AccessToken，优先获取缓存的
        /// </summary>
        public string AccessToken
        {
            get
            {
                string token = _cache.GetString(AccessTokenCacheKey);
                if (string.IsNullOrEmpty(token))
                {
                    var res = this.GetAccessToken().Result;
                    _cache.SetString(AccessTokenCacheKey, res.Access_token, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(res.Expires_in - 60)
                    });
                    token = res.Access_token;
                }
                return token;
            }
        }
        /// <summary>
        /// 获取AccessToken，优先获取缓存的
        /// </summary>
        public string JsapiTicket
        {
            get
            {
                string ticket = _cache.GetString(JsapiTicketCacheKey);
                if (string.IsNullOrEmpty(ticket))
                {
                    var res = this.GetJsApiTicket().Result;
                    _cache.SetString(JsapiTicketCacheKey, res.Ticket, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(res.Expires_in - 60)
                    });
                    ticket = res.Ticket;
                }
                return ticket;
            }
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
            if (result.Errcode == (int)ErrorCode.SUCCESS)
            {
                return result;
            }
            //token过期时，就重新发起请求
            if (result.Errcode == (int)ErrorCode.FAIL_TOKEN)
            {
                var token = await GetAccessToken();
                url = Regex.Replace(url, "access_token=[^&]*", $"access_token={token.Access_token}");
                return await this.RequestAsync<T>(url);
            }
            else
            {
                string message = $"Api接口错误：[{result.Errcode}:{result.Errmsg}]";
                if (Enum.IsDefined(typeof(ErrorCode), result.Errcode))
                {
                    var errorCode = (ErrorCode)result.Errcode;
                    var attributes = errorCode.GetType().GetField(errorCode.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes.Length > 0)
                    {
                        var attribute = attributes[0] as DescriptionAttribute;
                        message = attribute.Description;
                    }
                }
                throw new ApiFailException(ApiFailCode.API_FAIL, message);
            }
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public async Task<AccessToken> GetAccessToken()
        {
            var token = await RequestAsync<AccessToken>($"gettoken?corpid={_qyWeixinOption.CcorpId}&corpsecret={_qyWeixinOption.Secret}");
            return token;
        }

        /// <summary>
        /// 读取成员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string userid)
        {
            string url = $"user/get?access_token={this.AccessToken}&userid={userid}";
            return await this.RequestAsync<User>(url);
        }
        /// <summary>
        /// 构建认证Url
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public string BuildAuthUrl(string redirectUrl, string state = null)
        {
            redirectUrl = WebUtility.UrlEncode(redirectUrl);
            state = state ?? "STATE";
            string url = $"https://open.weixin.qq.com/connect/oauth2/authorize?appid={_qyWeixinOption.CcorpId}&redirect_uri={redirectUrl}&response_type=code&scope=snsapi_base&state={state}#wechat_redirect";
            return url;
        }

        /// <summary>
        /// 根据Code获取用户信息
        /// </summary>
        /// <param name="code"></param>
        public async Task<UserInfo> GetUserInfoByCodeAsync(string code)
        {
            string url = $"user/getuserinfo?access_token={this.AccessToken}&code={code}";
            return await this.RequestAsync<UserInfo>(url);
        }

        public async Task<JsapiTicket> GetJsApiTicket()
        {
            string url = $"get_jsapi_ticket?access_token={this.AccessToken}";
            return await this.RequestAsync<JsapiTicket>(url);
        }
        public WeixinJssdkConfigDto GetJssdkConfig(string url)
        {
            string s = "123456789abcdefghijklmnpqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ";
            var random = new Random(90);
            string noncestr = "";
            for (int i = 0; i < 15; i++)
            {
                int index = random.Next(0, s.Length);
                noncestr += s[index];
            }
            long timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string a = $"jsapi_ticket={this.JsapiTicket}&noncestr={noncestr}&timestamp={timestamp}&url={url}";
            var sha1 = SHA1.Create();
            string sign = BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(a))).Replace("-", "");
            return new WeixinJssdkConfigDto
            {
                AppId = _qyWeixinOption.CcorpId,
                Timestamp = timestamp,
                Signature = sign,
                NonceStr = noncestr
            };
        }
    }

    public enum ErrorCode
    {
        [Description("系统繁忙")]
        SYS_BUSY = -1,
        SUCCESS = 0,
        [Description("不合格的Secret")]
        FAIL_SECRET = 40001,
        [Description("过期的AccessToken")]
        FAIL_TOKEN = 40014,
        [Description("失效的Code值")]
        FAIL_CODE = 40029,
    }
}

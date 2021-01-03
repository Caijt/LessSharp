using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LessSharp.Common;
using LessSharp.Dto;

namespace LessSharp.WebApi.AuthenticationSchemes.ApiFail
{
    public class ApiFailHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static string FailCode = "FAIL_CODE";
        public static string FailMessage = "FAIL_MESSAGE";
        public ApiFailHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 没通过授权
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            ApiFailCode code = ApiFailCode.NO_PERMISSION;
            string message = null;
            if (this.Context.Items.ContainsKey(FailCode))
            {
                code = (ApiFailCode)this.Context.Items[FailCode];
            }
            if (this.Context.Items.ContainsKey(FailMessage))
            {
                message = this.Context.Items[FailMessage].ToString();
            }
            await Response.WriteAsync(CommonHelper.ObjectToJsonString(ApiResultDto.Fail(code, message)));
        }
        /// <summary>
        /// 没有登录信息或登录信息失效
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            ApiFailCode code = ApiFailCode.NO_LOGIN;
            if (this.Context.Items.ContainsKey(FailCode))
            {
                code = (ApiFailCode)this.Context.Items[FailCode];
            }
            await Response.WriteAsync(CommonHelper.ObjectToJsonString(ApiResultDto.Fail(code)));
        }
    }
}

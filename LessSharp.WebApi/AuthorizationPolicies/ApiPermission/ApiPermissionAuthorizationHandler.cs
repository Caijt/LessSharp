using LessSharp.Common;
using LessSharp.Service;
using LessSharp.WebApi.AuthenticationSchemes.ApiFail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LessSharp.WebApi.AuthorizationPolicies.ApiPermission
{
    public class ApiPermissionAuthorizationHandler : AuthorizationHandler<ApiPermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContext;
        public ApiPermissionAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiPermissionRequirement requirement)
        {
            if (context.User?.Identity != null && context.User.Identity.IsAuthenticated)
            {

                //var filterContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
                var authService = _httpContext.HttpContext.RequestServices.GetRequiredService<AuthService>();
                var checkResult = await authService.CheckApiPermissionByPathAsync(_httpContext.HttpContext.Request.Path);
                if (checkResult.IsSuccess)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    _httpContext.HttpContext.Items[ApiFailHandler.FailCode] = ApiFailCode.NO_API_PERMISSION;
                    if (!string.IsNullOrWhiteSpace(checkResult.ApiName))
                    {
                        _httpContext.HttpContext.Items[ApiFailHandler.FailMessage] = $"当前用户没有接口 [ {checkResult.ApiName} ] 的访问权限，请联系管理员开通。";
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}

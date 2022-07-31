using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessSharp.Dto;
using LessSharp.Dto.Sys;
using LessSharp.Service.Sys;
using LessSharp.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using LessSharp.WebApi.AuthorizationPolicies;

namespace LessSharp.WebApi.Controllers.Sys
{
    [OpenApiTag("Token管理")]
    [ApiGroup(ApiGroupNames.Sys)]
    [Authorize(AuthorizationPolicies.AuthorizationPolicyNames.ApiPermission)]
    public class TokenController : AutoRouteAuthorizeControllerBase
    {
        private readonly TokenService _userService;
        public TokenController(TokenService sysUserService)
        {
            _userService = sysUserService;
        }
        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<PageListDto<TokenDto>>> GetPageList([FromQuery] TokenQueryDto queryDto)
        {
            return await _userService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 根据Id值进行删除Token记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<ApiResultDto> DeleteById([FromBody] int id)
        //{
        //    await _userService.DeleteByIdAsync(id);
        //    return ApiResultDto.Success();
        //}

        /// <summary>
        /// 禁用Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> DisableToken(string  accessToken)
        {
            await _userService.DisableTokenAsync(accessToken);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 重置已禁用还未过期的Token到缓存中
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto> ReloadDisabledToken()
        {
            await _userService.ReloadDisabledTokenAsync();
            return ApiResultDto.Success();
        }
    }
}
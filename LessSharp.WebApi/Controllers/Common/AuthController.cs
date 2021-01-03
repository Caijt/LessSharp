using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessSharp.Dto;
using LessSharp.Dto.Common;
using LessSharp.Dto.Sys;
using LessSharp.Entity.Sys;
using LessSharp.Service;
using LessSharp.Service.Sys;
using LessSharp.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace LessSharp.WebApi.Controllers
{
    [OpenApiTag("登录授权")]
    [ApiGroup(ApiGroupNames.Common)]
    public class AuthController : AutoRouteControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserLoginLogService _userLoginLogService;
        public AuthController(AuthService authService, UserLoginLogService userLoginLogService)
        {
            _authService = authService;
            _userLoginLogService = userLoginLogService;
        }

        /// <summary>
        /// 登录获取认证Token
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResultDto<LoginResultDto>> Login(LoginDto loginDto)
        {
            return await _authService.GetTokenByLoginAsync(loginDto);
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto> Logout()
        {
            await _authService.LogoutAsync();
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 获取登录认证用户信息及菜单权限信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<AuthInfoDto>> GetAuthInfo()
        {
            return await _authService.GetAuthInfoAsync();
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResultDto<LoginResultDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            return await _authService.RefreshTokenAsync(refreshTokenDto);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> ChangePassword(ChangePasswordDto dto)
        {
            await _authService.ChangePasswordAsync(dto);
            return ApiResultDto.Success();
        }
        /// <summary>
        /// 获取用户登录日志分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<PageListDto<UserLoginLogDto>>> GetLoginLog([FromQuery]UserLoginLogQueryDto queryDto)
        {
            queryDto.IsLoginUser = true;
            return await _userLoginLogService.GetPageListAsync(queryDto);
        }
    }
}
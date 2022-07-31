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
using LessSharp.Entity.Sys;

namespace LessSharp.WebApi.Controllers.Sys
{
    [OpenApiTag("配置管理")]
    [ApiGroup(ApiGroupNames.Sys)]
    [Authorize(AuthorizationPolicies.AuthorizationPolicyNames.ApiPermission)]
    public class ConfigController : AutoRouteAuthorizeControllerBase
    {
        private readonly ConfigService _configService;
        public ConfigController(ConfigService configService)
        {
            _configService = configService;
        }


        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<List<ConfigDto>>> GetList([FromQuery] ConfigQueryDto queryDto)
        {
            return await _configService.GetListAsync(queryDto);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> Save(List<ConfigDto> dtos)
        {
            await _configService.SaveAsync(dtos);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 根据Key值获取配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<string>> GetValueByKey(string key)
        {
            return await _configService.GetValueByKey(key);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessSharp.Dto;
using LessSharp.Service;
using LessSharp.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using LessSharp.Service.Common;
using LessSharp.Dto.Sys;
using LessSharp.Service.Sys;

namespace LessSharp.WebApi.Controllers
{
    [OpenApiTag("公共服务")]
    [ApiGroup(ApiGroupNames.Common)]
    public class CommonController : AutoRouteControllerBase
    {
        private readonly CommonService _commonService;
        private readonly ConfigService _configService;
        public CommonController(CommonService commonService, ConfigService configService)
        {
            _commonService = commonService;
            _configService = configService;
        }
        /// <summary>
        /// 获取一个Guid值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResultDto<Guid> GetGuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResultDto<List<ConfigDto>>> GetConfigList([FromQuery] ConfigQueryDto dto)
        {
            return await _configService.GetListAsync(dto);
        }

        /// <summary>
        /// 根据Key值获取配置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResultDto<string>> GetConfigValueByKey(string key)
        {
            return await _configService.GetValueByKey(key);
        }
    }
}